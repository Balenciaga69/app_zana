using Microsoft.AspNetCore.SignalR;

namespace BasicApp.Chat.Hubs;

// This class handles chat communication between clients using SignalR.
public class ChatHub : Hub
{
    // This method sends a message from one user to all connected clients.
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // This method returns the unique connection ID of the current client.
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    // This method is called when a new client connects to the hub.
    // It adds the client to a group called "MyGroup".
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "MyGroup");
        await base.OnConnectedAsync();
    }

    // This method is called when a client disconnects from the hub.
    // It removes the client from the group "MyGroup".
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "MyGroup");
        await base.OnDisconnectedAsync(exception);
    }
}