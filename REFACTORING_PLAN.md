# Liz Monolithic 專案重構調整計畫

## 📋 重構目標

讓專案回歸純粹、提升開發體驗、降低過度工程複雜度

---

## 🔍 當前問題分析

基於程式碼掃描結果，發現以下過度工程問題：

### 1. Controller 層問題
- ✅ **發現**：每個 Action 都包含大量 try-catch、Log、OperationResult 轉換
- ✅ **發現**：Controller 違反「輕薄層」原則，變得厚重
- ✅ **發現**：重複的錯誤處理和 Log 邏輯

### 2. OperationResult 過度使用
- ✅ **發現**：連簡單的 CRUD 都包 OperationResult
- ✅ **發現**：每個 Handler 都回傳 OperationResult，增加開發負擔
- ✅ **發現**：錯誤處理過於複雜化

### 3. Log 系統雜訊
- ✅ **發現**：Middleware、Filter、Controller、Handler 都有 Log
- ✅ **發現**：重複記錄同一次請求
- ✅ **發現**：Info/Debug Log 產生大量雜訊

### 4. 驗證責任不明確
- ✅ **發現**：Command/Query 有 GetValidationError、IsValid 方法
- ✅ **發現**：驗證邏輯分散，難以統一管理

---

## 🎯 重構策略

### 階段一：Controller 精簡化
```csharp
// 重構前（厚重的 Controller）
[HttpPost("register")]
public async Task<ActionResult<ApiResponse<RegisterUserResult>>> RegisterUser([FromBody] RegisterUserRequest request)
{
    try
    {
        _logger.LogInfo("開始處理用戶註冊", new { request.DeviceFingerprint });
        
        var command = new RegisterUserCommand { ... };
        var operationResult = await _mediator.Send(command);
        
        if (operationResult.Success)
        {
            _logger.LogInfo("用戶註冊成功", new { operationResult.Data!.UserId });
            return Ok(ApiResponse<RegisterUserResult>.Ok(operationResult.Data, "註冊成功"));
        }
        else
        {
            _logger.LogWarn("用戶註冊失敗", new { operationResult.ErrorCode });
            return BadRequest(ApiResponse<RegisterUserResult>.Fail(operationResult.ErrorCode!.Value));
        }
    }
    catch (Exception ex)
    {
        _logger.LogError("用戶註冊異常", ex, new { request.DeviceFingerprint });
        return StatusCode(500, ApiResponse<RegisterUserResult>.Fail(ErrorCode.InternalServerError));
    }
}

// 重構後（純粹的 Controller）
[HttpPost("register")]
public async Task<RegisterUserResult> RegisterUser([FromBody] RegisterUserRequest request)
{
    var command = new RegisterUserCommand 
    { 
        DeviceFingerprint = request.DeviceFingerprint,
        // ... 其他屬性
    };
    
    return await _mediator.Send(command);
}
```

### 階段二：移除過度 OperationResult
```csharp
// 重構前
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, OperationResult<RegisterUserResult>>

// 重構後
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
```

### 階段三：統一例外處理
- **移除** Controller 內的 try-catch
- **保留** 全域 ErrorHandlingMiddleware
- **簡化** 錯誤回應格式

### 階段四：Log 系統簡化
- **保留** Middleware/Filter 的全域 Log
- **移除** Controller/Handler 內的業務 Log
- **只保留** 關鍵錯誤 Log

### 階段五：驗證統一化
- **移除** Command/Query 的 GetValidationError、IsValid
- **統一** 使用 FluentValidation 或內建 ModelState
- **Controller** 層統一處理驗證錯誤

---

## 🔧 具體重構檔案清單

### 需要重構的檔案

#### Controllers
- [ ] `Features/User/Controller/UserController.cs`
- [ ] `Features/Health/Controllers/HealthController.cs`

#### Command Handlers
- [ ] `Features/User/Commands/RegisterUserCommandHandler.cs`
- [ ] `Features/User/Commands/UpdateUserNicknameCommandHandler.cs`

#### Query Handlers
- [ ] `Features/User/Queries/GetUserByIdQueryHandler.cs`
- [ ] `Features/User/Queries/GetUserConnectionsQueryHandler.cs`

#### Commands/Queries
- [ ] `Features/User/Commands/RegisterUserCommand.cs`
- [ ] `Features/User/Commands/UpdateUserNicknameCommand.cs`

#### 基礎設施
- [ ] `Shared/Common/OperationResult.cs` (簡化使用)
- [ ] `Shared/Common/ApiResponse.cs` (由 Middleware 處理)

---

## 🎨 重構後的理想架構

### 1. Controller 範例
```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost("register")]
    public async Task<RegisterUserResult> RegisterUser([FromBody] RegisterUserCommand command)
        => await _mediator.Send(command);
    
    [HttpGet("{id:guid}")]
    public async Task<GetUserByIdResult?> GetUser(Guid id)
        => await _mediator.Send(new GetUserByIdQuery(id));
    
    [HttpPut("{id:guid}/nickname")]
    public async Task<UpdateUserNicknameResult> UpdateNickname(Guid id, [FromBody] UpdateNicknameRequest request)
        => await _mediator.Send(new UpdateUserNicknameCommand(id, request.Nickname));
}
```

### 2. Handler 範例
```csharp
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserRepository _userRepository;
    
    public RegisterUserCommandHandler(IUserRepository userRepository) 
        => _userRepository = userRepository;
    
    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 純粹的業務邏輯
        var existingUser = await _userRepository.GetByDeviceFingerprintAsync(request.DeviceFingerprint);
        
        if (existingUser != null)
        {
            existingUser.LastActiveAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(existingUser);
            return CreateResult(existingUser, false);
        }
        
        var newUser = new User
        {
            DeviceFingerprint = request.DeviceFingerprint,
            Nickname = GenerateRandomNickname(),
            // ...
        };
        
        await _userRepository.CreateAsync(newUser);
        return CreateResult(newUser, true);
    }
    
    private static RegisterUserResult CreateResult(User user, bool isNewUser) => new()
    {
        UserId = user.Id,
        Nickname = user.Nickname,
        IsNewUser = isNewUser,
        // ...
    };
}
```

### 3. Repository 保持不變（已經很乾淨）
```csharp
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context) => _context = context;
    
    public async Task<User?> GetByIdAsync(Guid id) 
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    
    // 其他方法保持簡潔
}
```

---

## 🚀 重構執行順序

### 第一步：Middleware/Filter 調整
1. 確保 `ErrorHandlingMiddleware` 能正確處理業務異常
2. 調整 `ApiResponseResultFilter` 自動包裝回應
3. 精簡 `ApiLoggingActionFilter` 的 Log 內容

### 第二步：移除 Controller 雜訊
1. 移除所有 try-catch（讓 Middleware 處理）
2. 移除多餘的 Log（讓 Filter 處理）
3. 簡化 Action 方法（直接 return Handler 結果）

### 第三步：簡化 Handler
1. 移除 OperationResult 包裝（非必要時）
2. 移除多餘的 Log
3. 專注於業務邏輯

### 第四步：統一驗證
1. 移除 Command/Query 的驗證方法
2. 統一使用 FluentValidation 或 ModelState
3. 在 Middleware 層統一處理驗證錯誤

### 第五步：測試與調整
1. 執行現有測試確保功能正常
2. 調整測試案例配合新架構
3. 團隊 Code Review

---

## 📝 Copilot/AI 友善化

### 1. 新增範本檔案
```
Templates/
├── ControllerTemplate.cs
├── CommandHandlerTemplate.cs
├── QueryHandlerTemplate.cs
└── RepositoryTemplate.cs
```

### 2. 更新 README
- 明確說明各層職責
- 提供開發範例
- 說明不要做什麼（反模式）

### 3. 加強註解
```csharp
/// <summary>
/// 用戶控制器 - 只負責接收請求和調用 Command/Query
/// 不要在這裡：加 try-catch、寫 Log、驗證邏輯、業務邏輯
/// </summary>
[ApiController]
public class UserController : ControllerBase
{
    // Copilot 會根據這些註解產生正確的程式碼
}
```

---

## ✅ 預期成果

### 開發體驗提升
- 寫一個新 API 只需要：Command + Handler + Controller Action
- 不需要處理 Log、try-catch、OperationResult 包裝
- Copilot 產生的程式碼更符合規範

### 程式碼品質提升
- Controller 回歸輕薄
- Handler 專注業務邏輯
- Repository 保持純粹資料存取

### 維護性提升
- 統一的錯誤處理
- 一致的 Log 格式
- 清晰的職責分工

---

## 🎯 開始重構！

請下一位 AI Agent 按照這個計畫執行重構，優先處理：
1. **UserController.cs** - 移除所有雜訊
2. **RegisterUserCommandHandler.cs** - 簡化業務邏輯
3. **UpdateUserNicknameCommandHandler.cs** - 移除 OperationResult

讓我們一起讓程式碼回歸純粹！🚀
