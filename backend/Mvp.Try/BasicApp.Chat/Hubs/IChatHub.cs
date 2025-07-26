namespace BasicApp.Chat.Hubs;

public interface IChatHub
{
    Task<string> RegisterUser(string? existingUserId = null);
    Task SendMessage(string user, string message);
    string GetConnectionId();
    Task<object> GetOnlineStats();
}
