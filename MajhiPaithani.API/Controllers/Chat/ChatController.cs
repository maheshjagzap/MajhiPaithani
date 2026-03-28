using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using MajhiPaithani.API.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MajhiPaithani.API.Controllers.Chat;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(ApplicationDbContext db, IHubContext<ChatHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }

    // ─────────────────────────────────────────────
    // POST /api/chat/send
    // ─────────────────────────────────────────────
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(r =>
            (r.ICustomerId == request.SenderId && r.ISellerId == request.ReceiverId) ||
            (r.ICustomerId == request.ReceiverId && r.ISellerId == request.SenderId));

        if (room == null)
        {
            room = new ChatRoom
            {
                ICustomerId = request.SenderId,
                ISellerId = request.ReceiverId,
                DCreatedDate = DateTime.UtcNow
            };
            _db.ChatRooms.Add(room);
            await _db.SaveChangesAsync();
        }

        var chatMessage = new ChatMessage
        {
            IChatRoomId = room.IChatRoomId,
            ISenderUserId = request.SenderId,
            IReceiverUserId = request.ReceiverId,
            SMessage = request.Message,
            DSentDate = DateTime.UtcNow,
            BIsDelivered = false,
            BIsRead = false
        };

        _db.ChatMessages.Add(chatMessage);
        await _db.SaveChangesAsync();

        var sentMessage = new
        {
            chatMessage.IMessageId,
            chatMessage.IChatRoomId,
            chatMessage.ISenderUserId,
            chatMessage.IReceiverUserId,
            chatMessage.SMessage,
            chatMessage.DSentDate,
            chatMessage.BIsDelivered,
            chatMessage.BIsRead
        };

        // Push new message to anyone with the chat window open
        await _hubContext.Clients.Group($"room_{room.IChatRoomId}").SendAsync("ReceiveMessage", sentMessage);

        // Push to receiver's personal channel (notification badge)
        await _hubContext.Clients.Group($"user_{request.ReceiverId}").SendAsync("NewMessageNotification", sentMessage);

        // Build conversation update payload for both users to refresh their inbox
        var senderUser = await _db.Users.Where(u => u.IUserId == request.SenderId)
            .Select(u => new { u.IUserId, u.SFirstName, u.SLastName }).FirstOrDefaultAsync();

        var receiverUser = await _db.Users.Where(u => u.IUserId == request.ReceiverId)
            .Select(u => new { u.IUserId, u.SFirstName, u.SLastName }).FirstOrDefaultAsync();

        var unreadCount = await _db.ChatMessages
            .CountAsync(m => m.IChatRoomId == room.IChatRoomId && m.IReceiverUserId == request.ReceiverId && !m.BIsRead);

        // For sender: otherUser is the receiver
        var conversationForSender = new
        {
            roomId = room.IChatRoomId,
            otherUserId = request.ReceiverId,
            otherUserName = receiverUser != null ? $"{receiverUser.SFirstName} {receiverUser.SLastName}".Trim() : "",
            lastMessage = request.Message,
            lastMessageTime = chatMessage.DSentDate,
            unreadCount = 0  // sender has no unread in their own sent message
        };

        // For receiver: otherUser is the sender
        var conversationForReceiver = new
        {
            roomId = room.IChatRoomId,
            otherUserId = request.SenderId,
            otherUserName = senderUser != null ? $"{senderUser.SFirstName} {senderUser.SLastName}".Trim() : "",
            lastMessage = request.Message,
            lastMessageTime = chatMessage.DSentDate,
            unreadCount
        };

        await _hubContext.Clients.Group($"user_{request.SenderId}").SendAsync("ConversationUpdated", conversationForSender);
        await _hubContext.Clients.Group($"user_{request.ReceiverId}").SendAsync("ConversationUpdated", conversationForReceiver);

        return Ok(new { roomId = room.IChatRoomId, sentMessage });
    }

    // ─────────────────────────────────────────────
    // GET /api/chat/history
    // Supports both:
    //   ?roomId=5                        (primary — use this when you already have roomId)
    //   ?senderId=101&receiverId=55      (fallback — for first open before roomId is known)
    // ─────────────────────────────────────────────
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int? roomId,
        [FromQuery] int? senderId,
        [FromQuery] int? receiverId)
    {
        ChatRoom? room = null;

        if (roomId.HasValue)
        {
            room = await _db.ChatRooms.FirstOrDefaultAsync(r => r.IChatRoomId == roomId.Value);
        }
        else if (senderId.HasValue && receiverId.HasValue)
        {
            room = await _db.ChatRooms.FirstOrDefaultAsync(r =>
                (r.ICustomerId == senderId && r.ISellerId == receiverId) ||
                (r.ICustomerId == receiverId && r.ISellerId == senderId));
        }
        else
        {
            return BadRequest("Provide either roomId or both senderId and receiverId.");
        }

        if (room == null)
            return Ok(new { roomId = (int?)null, messages = Array.Empty<object>() });

        var messages = await _db.ChatMessages
            .Where(m => m.IChatRoomId == room.IChatRoomId)
            .OrderBy(m => m.DSentDate)
            .Select(m => new
            {
                m.IMessageId,
                m.IChatRoomId,
                m.ISenderUserId,
                m.IReceiverUserId,
                m.SMessage,
                m.DSentDate,
                m.BIsDelivered,
                m.BIsRead
            })
            .ToListAsync();

        return Ok(new { roomId = room.IChatRoomId, messages });
    }

    // ─────────────────────────────────────────────
    // GET /api/chat/conversations?userId=101
    // Returns all conversations for a user, sorted by latest message DESC
    // ─────────────────────────────────────────────
    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations([FromQuery] int userId)
    {
        // Get all rooms where this user is either customer or seller side
        var rooms = await _db.ChatRooms
            .Where(r => r.ICustomerId == userId || r.ISellerId == userId)
            .ToListAsync();

        if (!rooms.Any())
            return Ok(new List<object>());

        var roomIds = rooms.Select(r => r.IChatRoomId).ToList();

        // Get latest message per room in one query
        var latestMessages = await _db.ChatMessages
            .Where(m => m.IChatRoomId != null && roomIds.Contains(m.IChatRoomId.Value))
            .GroupBy(m => m.IChatRoomId)
            .Select(g => g.OrderByDescending(m => m.DSentDate).First())
            .ToListAsync();

        // Get unread counts per room for this user
        var unreadCounts = await _db.ChatMessages
            .Where(m => m.IChatRoomId != null && roomIds.Contains(m.IChatRoomId.Value)
                     && m.IReceiverUserId == userId && !m.BIsRead)
            .GroupBy(m => m.IChatRoomId)
            .Select(g => new { RoomId = g.Key, Count = g.Count() })
            .ToListAsync();

        // Collect all other user IDs to fetch names in one query
        var otherUserIds = rooms
            .Select(r => r.ICustomerId == userId ? r.ISellerId : r.ICustomerId)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        var users = await _db.Users
            .Where(u => otherUserIds.Contains(u.IUserId))
            .Select(u => new { u.IUserId, u.SFirstName, u.SLastName })
            .ToListAsync();

        var unreadDict = unreadCounts.ToDictionary(x => x.RoomId ?? 0, x => x.Count);
        var userDict = users.ToDictionary(u => u.IUserId);

        var conversations = rooms
            .Select(room =>
            {
                var otherUserId = room.ICustomerId == userId ? room.ISellerId : room.ICustomerId;
                var latest = latestMessages.FirstOrDefault(m => m.IChatRoomId == room.IChatRoomId);
                var unread = unreadDict.TryGetValue(room.IChatRoomId, out var count) ? count : 0;
                var otherUser = otherUserId.HasValue && userDict.TryGetValue(otherUserId.Value, out var u) ? u : null;

                return new
                {
                    roomId = room.IChatRoomId,
                    otherUserId = otherUserId ?? 0,
                    otherUserName = otherUser != null ? $"{otherUser.SFirstName} {otherUser.SLastName}".Trim() : "Unknown",
                    lastMessage = latest?.SMessage ?? "",
                    lastMessageTime = latest?.DSentDate,
                    unreadCount = unread
                };
            })
            .Where(c => c.lastMessageTime.HasValue)   // exclude rooms with no messages yet
            .OrderByDescending(c => c.lastMessageTime)
            .ToList<object>();

        return Ok(conversations);
    }
}

public record SendMessageRequest(int SenderId, int ReceiverId, string Message);
