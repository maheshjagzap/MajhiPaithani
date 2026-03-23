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

    /// <summary>
    /// PURPOSE: Send a message from one user to another.
    /// Internally checks if a chat room already exists between sender and receiver:
    ///   - If YES => reuses the existing room
    ///   - If NO  => creates a new room automatically
    /// Saves the message to DB, broadcasts it via SignalR, and returns roomId + sentMessage.
    ///
    /// Example:
    ///   POST /api/chat/send
    ///   { "senderId": 1, "receiverId": 4, "message": "Hello!" }
    ///   => { roomId: 1, sentMessage: { ... } }
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        // Step 1: Find existing room between these two users (in either direction)
        var room = await _db.ChatRooms.FirstOrDefaultAsync(r =>
            (r.ICustomerId == request.SenderId && r.ISellerId == request.ReceiverId) ||
            (r.ICustomerId == request.ReceiverId && r.ISellerId == request.SenderId));

        // Step 2: No room found — create one now
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

        // Step 3: Save the message linked to the room
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

        // Step 4: Push the new message in real-time to all SignalR clients in this room
        await _hubContext.Clients.Group(room.IChatRoomId.ToString()).SendAsync("ReceiveMessage", sentMessage);

        return Ok(new { roomId = room.IChatRoomId, sentMessage });
    }

    /// <summary>
    /// PURPOSE: Load full chat history between two users when they open the chat window.
    /// Returns roomId + all past messages ordered oldest to newest.
    /// Returns roomId as null and empty messages if they have never chatted before.
    ///
    /// Example:
    ///   GET /api/chat/history?senderId=1&receiverId=4
    ///   => { roomId: 1, messages: [...] }
    ///   => { roomId: null, messages: [] }  (first time chatting)
    /// </summary>
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int senderId, [FromQuery] int receiverId)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(r =>
            (r.ICustomerId == senderId && r.ISellerId == receiverId) ||
            (r.ICustomerId == receiverId && r.ISellerId == senderId));

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

    //    [HttpPost("mark-delivered")]
    //    public async Task<IActionResult> MarkDelivered([FromBody] MarkStatusRequest request)
    //    {
    //        var messages = await _db.ChatMessages
    //            .Where(m => m.IChatRoomId == request.RoomId && m.IReceiverUserId == request.UserId && !m.BIsDelivered)
    //            .ToListAsync();

    //        if (!messages.Any()) return Ok();

    //        foreach (var m in messages)
    //        {
    //            m.BIsDelivered = true;
    //            m.DDeliveredDate = DateTime.UtcNow;
    //        }
    //        await _db.SaveChangesAsync();

    //        var ids = messages.Select(m => m.IMessageId).ToList();
    //        await _hubContext.Clients.Group(request.RoomId.ToString()).SendAsync("MessagesDelivered", ids);
    //        return Ok();
    //    }

    //    [HttpPost("mark-read")]
    //    public async Task<IActionResult> MarkRead([FromBody] MarkStatusRequest request)
    //    {
    //        var messages = await _db.ChatMessages
    //            .Where(m => m.IChatRoomId == request.RoomId && m.IReceiverUserId == request.UserId && !m.BIsRead)
    //            .ToListAsync();

    //        if (!messages.Any()) return Ok();

    //        foreach (var m in messages)
    //        {
    //            m.BIsRead = true;
    //            m.BIsDelivered = true;
    //            m.DReadDate = DateTime.UtcNow;
    //            m.DDeliveredDate ??= DateTime.UtcNow;
    //        }
    //        await _db.SaveChangesAsync();

    //        var ids = messages.Select(m => m.IMessageId).ToList();
    //        await _hubContext.Clients.Group(request.RoomId.ToString()).SendAsync("MessagesRead", ids);
    //        return Ok();
    //    }
    //}
}
public record SendMessageRequest(int SenderId, int ReceiverId, string Message);
public record MarkStatusRequest(int RoomId, int UserId);
