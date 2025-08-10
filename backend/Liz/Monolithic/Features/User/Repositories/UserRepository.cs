namespace Monolithic.Features.User.Repositories;
using User = Infrastructure.Data.Entities.User;

public class UserRepository : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid userId)
    {
        // 查詢資料庫，根據 userId 取得 User 實體，找不到則回傳 null
        // 注意：
        // 1. userId 必須是唯一主鍵，查詢時要考慮不存在的情境。
        // 2. 請確保查詢時不會追蹤（NoTracking），除非需要更新。
        // 3. 若有快取，可優先查快取再查資料庫。
        throw new NotImplementedException();
    }

    public Task<User?> GetByDeviceFingerprintAsync(string deviceFingerprint)
    {
        // 查詢資料庫，根據 deviceFingerprint 取得 User 實體，找不到則回傳 null
        // 注意：
        // 1. deviceFingerprint 應唯一對應一個 User。
        // 2. 請考慮 deviceFingerprint 的唯一性與索引效能。
        // 3. 若有快取，可優先查快取。
        throw new NotImplementedException();
    }

    public Task<User> CreateAsync(User user)
    {
        // 將新的 User 實體新增到資料庫，儲存後回傳該 User
        // 注意：
        // 1. deviceFingerprint 不可重複，請先檢查是否已存在。
        // 2. 設定 CreatedAt、UpdatedAt 時間戳。
        // 3. 若有快取，新增後要同步快取。
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        // 更新資料庫中的 User 實體（如暱稱、活躍狀態、最後活動時間等）
        // 注意：
        // 1. 更新 UpdatedAt 時間戳。
        // 2. 若有快取，更新後要同步快取。
        // 3. 請考慮部分欄位更新（Patch）與全量更新（Put）的差異。
        throw new NotImplementedException();
    }

    public Task<bool> IsUserOnlineAsync(Guid userId)
    {
        // 檢查指定 userId 是否有活躍連線（可查 UserConnection 或快取）
        // 注意：
        // 1. 通常需查 UserConnection 是否有 DisconnectedAt 為 null 的紀錄。
        // 2. 若有 Redis 快取，建議查快取提升效能。
        // 3. 請考慮多連線情境（同一 userId 多個連線）。
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetOnlineUsersAsync()
    {
        // 查詢所有目前在線的 User（可根據 UserConnection 或快取判斷）
        // 注意：
        // 1. 需先查出所有活躍 UserConnection，再反查 User。
        // 2. 若有快取，建議查快取。
        // 3. 請考慮效能與資料一致性。
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetInactiveUsersAsync(DateTime inactiveSince)
    {
        // 查詢自指定時間點以來未活躍的 User（LastActiveAt < inactiveSince）
        // 注意：
        // 1. 請確保 LastActiveAt 欄位有正確索引。
        // 2. 查詢結果可能很大，建議分頁。
        // 3. 用於背景清理服務時，請考慮批次處理。
        throw new NotImplementedException();
    }
}
