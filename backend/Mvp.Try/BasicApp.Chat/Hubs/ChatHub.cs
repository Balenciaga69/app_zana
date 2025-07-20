using Microsoft.AspNetCore.SignalR;

namespace BasicApp.Chat.Hubs;

// This class handles chat communication between clients using SignalR.
public class ChatHub : Hub
{
    // This method sends a message from one user to all connected clients.
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine($"!!! {user} 說: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // This method returns the unique connection ID of the current client.
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}