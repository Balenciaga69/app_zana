using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class UserConnection : BaseEntity
{
    [Required]
    public string UserId { get; set; } = default!; // 用戶 ID

    [Required]
    public string ConnectionId { get; set; } = default!; // 用戶連接 ID
    public DateTime ConnectedAt { get; set; } // 用戶連接時間
    public DateTime? DisconnectedAt { get; set; } // 用戶斷開連接時間
    public string? IpAddress { get; set; } // 用戶的 IP 地址
    public string? UserAgent { get; set; } // 用戶的 User-Agent 字符串
}
