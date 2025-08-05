# MediatR 實作指南

## 概述
本專案已成功導入 MediatR，用於實現 CQRS (Command Query Responsibility Segregation) 模式，解耦 Controller 與 Service 之間的直接依賴。

## 設定完成
✅ **MediatR 套件已安裝**
- MediatR (10.0.0)
- MediatR.Extensions.Microsoft.DependencyInjection (10.0.0)

✅ **服務註冊已完成**
- `Program.cs` 中已註冊 MediatR 服務
- 使用 `AddMediatRServices()` 擴展方法
- 自動掃描當前組件中的所有 Handler

✅ **資料夾結構已建立**
```
Features/
├── Identity/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Requests/        ← 新增
│   └── Handlers/        ← 新增
└── Health/
    ├── Controllers/
    ├── Requests/        ← 新增
    └── Handlers/        ← 新增
```

## 實作範例

### 1. Command 範例 (CreateOrRetrieveUserCommand)
```csharp
// Requests/CreateOrRetrieveUserCommand.cs
public class CreateOrRetrieveUserCommand : IRequest<UserSession>
{
    public string? BrowserFingerprint { get; set; }
    public string UserAgent { get; set; } = string.Empty;
    // ...其他屬性
}
```

### 2. Query 範例 (GetUserByIdQuery)
```csharp
// Requests/GetUserByIdQuery.cs
public class GetUserByIdQuery : IRequest<UserSession?>
{
    public Guid UserId { get; set; }
    
    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
```

### 3. Handler 範例
```csharp
// Handlers/CreateOrRetrieveUserHandler.cs
public class CreateOrRetrieveUserHandler : IRequestHandler<CreateOrRetrieveUserCommand, UserSession>
{
    private readonly IIdentityService _identityService;
    private readonly IAppLogger<CreateOrRetrieveUserHandler> _logger;

    public async Task<UserSession> Handle(CreateOrRetrieveUserCommand request, CancellationToken cancellationToken)
    {
        // 業務邏輯實作
        return await _identityService.CreateOrRetrieveUserAsync(createUserRequest);
    }
}
```

### 4. Controller 使用範例
```csharp
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-or-retrieve-mediatr")]
    public async Task<ActionResult<ApiResponse<UserSession>>> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = new CreateOrRetrieveUserCommand
        {
            BrowserFingerprint = request.BrowserFingerprint,
            // ...對應屬性
        };

        var result = await _mediator.Send(command);
        return Ok(ApiResponse<UserSession>.Ok(result));
    }
}
```

## 已實作的範例端點

### Identity 相關
- `GET /api/Identity/{userId}` - 使用 GetUserByIdQuery (MediatR)
- `POST /api/Identity/create-or-retrieve-mediatr` - 使用 CreateOrRetrieveUserCommand (MediatR)

### Health 相關
- `GET /Health/mediatr?includeDetails=true` - 使用 GetHealthStatusQuery (MediatR)

## 接下來的步驟

### 建議優先導入的功能
1. **Identity 功能完善**
   - FindUserByFingerprintQuery
   - ValidateUserCommand
   - UpdateUserActivityCommand

2. **Room 相關功能** (未來新增)
   - CreateRoomCommand
   - JoinRoomCommand
   - GetRoomQuery

3. **Message 相關功能** (未來新增)
   - SendMessageCommand
   - GetMessagesQuery

### 橫切面功能 (Pipeline Behaviors)
可考慮實作以下 Pipeline Behaviors：
- **LoggingBehavior**: 統一記錄請求/回應
- **ValidationBehavior**: 使用 FluentValidation 驗證
- **TransactionBehavior**: 資料庫異動管理
- **CachingBehavior**: 查詢結果快取

## 最佳實務

1. **命名慣例**
   - Command: `{動詞}{名詞}Command` (如 CreateUserCommand)
   - Query: `Get{名詞}Query` (如 GetUserQuery)
   - Handler: `{Request名稱}Handler`

2. **資料夾組織**
   - Requests 放置所有 Command/Query
   - Handlers 放置對應的處理邏輯
   - 保持一對一關係

3. **漸進式導入**
   - 先導入複雜業務邏輯
   - 保留現有 Service 以利平滑過渡
   - 逐步將舊有呼叫轉移至 MediatR

4. **測試策略**
   - 直接測試 Handler 邏輯
   - Mock IMediator 介面進行 Controller 測試
   - 利用 MediatR 的解耦優勢提升可測試性

## 注意事項
- 目前 IdentityService 中的方法尚未實作 (拋出 NotImplementedException)
- 建議先完成 Service 實作，再進行 MediatR 遷移
- 可同時保持 Service 直接呼叫與 MediatR 呼叫，逐步遷移


我同意你的做法，
1. Controller 只負責「接收請求」與「回應」
2. Request/Handler 明確分工
3. Service/Repository 專注資料存取或複雜邏輯
4. Pipeline Behavior 統一攔截(這個延後)
