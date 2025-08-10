namespace Monolithic.Features.User.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserConnection = Infrastructure.Data.Entities.UserConnection;

public class UserConnectionRepository : IUserConnectionRepository
{
    public Task<UserConnection?> GetByConnectionIdAsync(string connectionId)
    {
        // 查詢資料庫，根據 connectionId 取得 UserConnection 實體，找不到則回傳 null
        // 注意：
        // 1. connectionId 應唯一對應一個連線紀錄。
        // 2. 查詢時建議 NoTracking，除非要更新。
        // 3. 若有快取，優先查快取。
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserConnection>> GetConnectionsByUserIdAsync(Guid userId)
    {
        // 查詢資料庫，取得指定 userId 的所有 UserConnection 紀錄
        // 注意：
        // 1. 需考慮多連線情境（同一 userId 多個連線）。
        // 2. 查詢結果可能很多，建議分頁。
        // 3. 若有快取，優先查快取。
        throw new NotImplementedException();
    }

    public Task AddConnectionAsync(UserConnection connection)
    {
        // 新增一筆 UserConnection 連線紀錄到資料庫
        // 注意：
        // 1. 設定 ConnectedAt、CreatedAt、UpdatedAt 時間戳。
        // 2. connectionId 必須唯一。
        // 3. 若有快取，新增後要同步快取。
        throw new NotImplementedException();
    }

    public Task UpdateConnectionAsync(UserConnection connection)
    {
        // 更新資料庫中的 UserConnection 實體（如斷線時間、UserAgent、IP 等）
        // 注意：
        // 1. 更新 UpdatedAt 時間戳。
        // 2. 若有快取，更新後要同步快取。
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserConnection>> GetActiveConnectionsAsync()
    {
        // 查詢所有目前在線的 UserConnection（DisconnectedAt 為 null）
        // 注意：
        // 1. 需考慮效能，建議加索引。
        // 2. 若有快取，優先查快取。
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserConnection>> GetDisconnectedConnectionsAsync(DateTime disconnectedBefore)
    {
        // 查詢所有在指定時間前已斷線的 UserConnection（DisconnectedAt < disconnectedBefore）
        // 注意：
        // 1. 用於背景清理服務，建議分批查詢。
        // 2. 若有快取，查詢後同步清理快取。
        throw new NotImplementedException();
    }

    public Task RemoveConnectionAsync(string connectionId)
    {
        // 移除指定 connectionId 的 UserConnection 紀錄
        // 注意：
        // 1. 移除前可先查詢確認存在。
        // 2. 若有快取，移除後要同步快取。
        throw new NotImplementedException();
    }
}
