using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Infrastructure.Data.Entities;

/// <summary>
/// 設備指紋實體 - 記錄用戶設備的唯一識別資訊
/// </summary>
public class DeviceFingerprint : BaseEntity
{
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 瀏覽器指紋（唯一識別）
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string BrowserFingerprint { get; set; } = string.Empty;

    /// <summary>
    /// 設備名稱（用戶可自定義）
    /// </summary>
    [MaxLength(100)]
    public string? DeviceName { get; set; }

    /// <summary>
    /// 設備類型
    /// </summary>
    [MaxLength(50)]
    public string? DeviceType { get; set; }

    /// <summary>
    /// 作業系統
    /// </summary>
    [MaxLength(100)]
    public string? OperatingSystem { get; set; }

    /// <summary>
    /// 瀏覽器名稱
    /// </summary>
    [MaxLength(100)]
    public string? Browser { get; set; }

    /// <summary>
    /// 瀏覽器版本
    /// </summary>
    [MaxLength(50)]
    public string? BrowserVersion { get; set; }

    /// <summary>
    /// 平台資訊
    /// </summary>
    [MaxLength(100)]
    public string? Platform { get; set; }

    /// <summary>
    /// 最後活動時間
    /// </summary>
    [Required]
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 是否為當前活躍設備
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 設備首次註冊時間
    /// </summary>
    [Required]
    public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 是否為信任設備
    /// </summary>
    public bool IsTrusted { get; set; } = false;

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
