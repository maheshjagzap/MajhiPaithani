using Microsoft.AspNetCore.SignalR;

namespace MajhiPaithani.API.Hubs;

public class ChatHub : Hub
{
    /// <summary>
    /// Called automatically when a user connects.
    /// React passes userId as query param: connection.withUrl("/hubs/chat?userId=101")
    /// User joins their personal channel so they receive messages even without opening a chat window.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (!string.IsNullOrEmpty(userId))
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Optional: React can call this when opening a specific chat window.
    /// Allows receiving messages scoped to that room (useful if user has multiple chat windows open).
    /// </summary>
    public async Task JoinRoom(int chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{chatRoomId}");
    }

    public async Task LeaveRoom(int chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room_{chatRoomId}");
    }
}
