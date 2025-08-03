# Identity 功能 Entity 與架構修改總結

## 已完成的修改

### 1. Entity 修改

#### User Entity 擴展
- 新增瀏覽器指紋相關欄位：
  - `BrowserFingerprint` - 瀏覽器指紋（255字元）
  - `UserAgent` - 用戶代理字串（500字元）
  - `IpAddress` - IP 位址（45字元，支援 IPv6）
  - `DeviceType` - 設備類型（50字元）
  - `OperatingSystem` - 作業系統（100字元）
  - `Browser` - 瀏覽器名稱（100字元）
  - `BrowserVersion` - 瀏覽器版本（50字元）
  - `Platform` - 平台資訊（100字元）

#### Connection Entity 擴展
- 新增連線時的設備資訊快照：
  - `UserAgent` - 連線時的用戶代理（500字元）
  - `IpAddress` - 連線時的 IP 位址（45字元）
  - `BrowserFingerprint` - 連線時的瀏覽器指紋（255字元）

### 2. EF Core 配置更新

#### UserConfiguration
- 新增 `BrowserFingerprint` 索引，用於快速查找用戶
- 設定所有新增欄位的最大長度限制
- 設定 `IsOnline` 預設值為 false

#### ConnectionConfiguration
- 設定設備資訊欄位的最大長度限制

### 3. Feature 架構建立

#### Identity Feature Models
- `CreateUserRequest` - 建立用戶請求模型
- `UserSession` - 用戶會話資訊模型
- `ValidateUserRequest` - 驗證用戶請求模型
- `FindUserByFingerprintRequest` - 根據指紋查找用戶請求模型

#### Identity Service
- `IIdentityService` 介面定義
- `IdentityService` 實作類別（含 TODO 架構規劃）

#### Identity Controller
- RESTful API 端點設計
- 統一的 `ApiResponse` 回應格式
- 完整的錯誤處理與日誌記錄

### 4. 服務註冊
- 建立 `ServiceCollectionExtensions.Identity.cs` 擴展方法
- 在 `Program.cs` 中註冊 Identity 相關服務

## 支援的功能需求

### ✅ 玩家使用同一瀏覽器就會使用同一 User
- 透過 `localStorage` 存儲 UserId
- 瀏覽器指紋作為備用識別機制
- 支援斷線重連時找回身份

### ✅ 每個 User 可以同時在多個房間聊天
- `RoomParticipant` Entity 支援一對多關係
- 複合唯一索引確保同一用戶在同一房間只有一個活躍參與記錄
- 支援用戶同時參與多個不同房間

### ✅ 設備與 IP 資訊記錄
- User Entity 記錄主要設備資訊
- Connection Entity 記錄每次連線的設備快照
- 支援多種設備資訊類型的儲存

## 身份識別流程

1. **首次進入**：前端生成 UserId + 收集指紋 → 後端建立新 User
2. **後續進入**：前端讀取 UserId + 收集指紋 → 後端驗證身份
3. **輔助找回**：localStorage 遺失時，透過指紋嘗試找回用戶
4. **設備記錄**：每次連線更新用戶與連線的設備資訊

## 下一步開發重點

### 需要實作的功能
1. `IdentityService` 的具體實作邏輯
2. 瀏覽器指紋比對演算法
3. 用戶身份驗證機制
4. 斷線重連邏輯
5. 設備資訊更新機制

### 需要的資料庫遷移
- 執行 `dotnet ef migrations add "AddUserDeviceInfo"` 來產生遷移檔案
- 執行 `dotnet ef database update` 來更新資料庫結構
