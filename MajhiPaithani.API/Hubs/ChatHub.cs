using Microsoft.AspNetCore.SignalR;

namespace MajhiPaithani.API.Hubs;

/// <summary>
/// SignalR Hub — handles real-time WebSocket connections.
/// Responsible ONLY for room management and broadcasting messages.
/// DB saving is handled by ChatController (REST endpoint).
/// </summary>
public class ChatHub : Hub
{
    /// <summary>
    /// PURPOSE: A user joins a chat room group so they can receive real-time messages.
    /// Both sender and receiver must call JoinRoom with the same chatRoomId.
    /// Call this right after connecting to the hub.
    /// </summary>
    public async Task JoinRoom(int chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }

    /// <summary>
    /// PURPOSE: A user leaves the chat room (e.g. closes the chat window).
    /// After this they will no longer receive real-time messages for this room.
    /// </summary>
    public async Task LeaveRoom(int chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }

    /// <summary>
    /// PURPOSE: Broadcast a message to everyone in the room in real-time.
    /// NOTE: DB saving is done by POST /api/chat/messages/send (REST endpoint).
    /// The HTML page calls the REST endpoint first (saves to DB + triggers this broadcast via IHubContext),
    /// so this method is only needed if you want to send directly via WebSocket without REST.
    /// </summary>
    public async Task SendMessage(int chatRoomId, int senderId, string message)
    {
        await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", new
        {
            iMessageId = 0,
            iChatRoomId = chatRoomId,
            iSenderUserId = senderId,
            sMessage = message,
            dSentDate = DateTime.UtcNow
        });
    }
}
