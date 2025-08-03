# 統一 Logger 工具使用說明

## 概述

已建立統一的 Logger 工具 `IAppLogger<T>`，提供結構化、分類明確的日誌記錄功能，替代原生 `ILogger<T>`。

## 主要特色

### 🎯 統一格式
- 所有日誌都有固定格式：`[日誌類型] 服務名稱 | 具體資訊 | 追蹤ID`
- 自動注入 TraceId，方便跨服務追蹤
- 結構化參數記錄，便於查詢和分析

### 🏷️ 分類明確
- **用戶操作日誌**：記錄用戶行為
- **業務邏輯日誌**：記錄業務流程
- **API 請求/回應日誌**：記錄 HTTP 交互
- **資料庫操作日誌**：記錄資料存取
- **效能監控日誌**：記錄執行時間
- **安全事件日誌**：記錄安全相關事件

### 🔍 易於搜尋
- 使用 `LogOperations` 常數，避免字串錯誤
- 結構化資料記錄，便於 ELK Stack 分析
- 自動 TraceId 注入，支援分散式追蹤

## 使用方式

### 1. 依賴注入

```csharp
// Service 中
public class IdentityService : IIdentityService
{
    private readonly IAppLogger<IdentityService> _appLogger;

    public IdentityService(IAppLogger<IdentityService> appLogger)
    {
        _appLogger = appLogger;
    }
}

// Controller 中
public class IdentityController : ControllerBase
{
    private readonly IAppLogger<IdentityController> _appLogger;

    public IdentityController(IAppLogger<IdentityController> appLogger)
    {
        _appLogger = appLogger;
    }
}
```

### 2. 常用方法範例

```csharp
// 用戶操作日誌
_appLogger.LogUserAction(userId, LogOperations.UserCreated, new { DeviceInfo = deviceInfo });

// 業務邏輯日誌
_appLogger.LogBusinessInfo(LogOperations.UserRetrieved, new { UserId = userId });

// 業務警告
_appLogger.LogBusinessWarning(LogOperations.UserValidated, "指紋不匹配", new { UserId = userId });

// 業務錯誤
_appLogger.LogBusinessError(LogOperations.UserCreated, exception, requestData);

// API 請求日誌
_appLogger.LogApiRequest("POST", "create-user", requestData, traceId);

// API 回應日誌
_appLogger.LogApiResponse("POST", "create-user", 200, duration, traceId);

// 資料庫操作日誌
_appLogger.LogDatabaseOperation("SELECT", "User", userId, duration);

// 效能監控
_appLogger.LogPerformance("UserCreation", duration, metadata);

// 安全事件
_appLogger.LogSecurityEvent(SecurityEvents.FingerprintMismatch, details, userId, ipAddress);
```

### 3. 實際應用範例

```csharp
public async Task<UserSession> CreateOrRetrieveUserAsync(CreateUserRequest request)
{
    var stopwatch = Stopwatch.StartNew();
    
    try
    {
        // 記錄業務開始
        _appLogger.LogBusinessInfo(LogOperations.UserCreated, new { request.BrowserFingerprint, request.IpAddress });

        // 查詢資料庫
        var user = await _context.Users.FirstOrDefaultAsync(u => u.BrowserFingerprint == request.BrowserFingerprint);
        
        _appLogger.LogDatabaseOperation("SELECT", "User", request.BrowserFingerprint, stopwatch.Elapsed);

        if (user == null)
        {
            // 建立新用戶
            user = new User { /* ... */ };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            _appLogger.LogDatabaseOperation("INSERT", "User", user.Id, stopwatch.Elapsed);
            _appLogger.LogUserAction(user.Id, LogOperations.UserCreated, request);
        }
        else
        {
            _appLogger.LogUserAction(user.Id, LogOperations.UserRetrieved, request);
        }

        // 記錄效能
        _appLogger.LogPerformance("CreateOrRetrieveUser", stopwatch.Elapsed, new { UserId = user.Id });

        return MapToUserSession(user);
    }
    catch (Exception ex)
    {
        _appLogger.LogBusinessError(LogOperations.UserCreated, ex, request);
        throw;
    }
}
```

## 日誌輸出範例

```json
{
  "timestamp": "2025-08-04T10:30:45.123Z",
  "level": "Information",
  "message": "[用戶操作] IdentityService | 用戶: 12345678-1234-1234-1234-123456789012 | 操作: 用戶建立 | 資料: {\"BrowserFingerprint\":\"abc123\",\"IpAddress\":\"192.168.1.1\"} | 追蹤: tr-abc12345",
  "properties": {
    "ServiceName": "IdentityService",
    "UserId": "12345678-1234-1234-1234-123456789012",
    "Action": "用戶建立",
    "TraceId": "tr-abc12345"
  }
}
```

## 常用常數

### LogOperations
- `UserCreated`, `UserRetrieved`, `UserValidated`
- `DatabaseQuery`, `DatabaseInsert`, `DatabaseUpdate`
- `ApiRequestReceived`, `ApiResponseSent`
- `ConnectionEstablished`, `ConnectionClosed`

### SecurityEvents
- `SuspiciousActivity`, `UnauthorizedAccess`
- `FingerprintMismatch`, `MultipleFailedAttempts`

## 優勢

1. **一致性**：所有服務使用相同格式，便於維護
2. **可搜索性**：結構化資料便於 ELK Stack 分析
3. **可追蹤性**：自動 TraceId 支援分散式追蹤
4. **分類明確**：不同類型日誌有明確區分
5. **擴展性**：未來可輕鬆添加新的日誌類型
6. **效能導向**：自動記錄執行時間，便於效能優化

## 注意事項

- 敏感資料（如密碼、Token）不要記錄在日誌中
- 大量資料建議只記錄關鍵欄位，避免日誌過大
- 在 catch 區塊中務必記錄錯誤日誌
- TraceId 會自動從 HTTP Context 取得，手動傳入時優先使用手動值
