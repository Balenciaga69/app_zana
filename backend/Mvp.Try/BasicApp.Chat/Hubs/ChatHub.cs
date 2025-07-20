using Microsoft.AspNetCore.SignalR;

namespace BasicApp.Chat.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine($"Received message from {user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
