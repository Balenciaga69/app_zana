好的！讓我制定詳細的 User Feature 開發計畫。讓我先檢查當前的項目結構，然後給你具體的步驟。 好的！我看到 User Feature 的資料夾結構已存在但是空的。以下是我的詳細開發計畫：

## 🎯 User Feature 開發計畫

### 第一階段：核心 Commands 與 Queries（預估 1-2 小時）

#### 步驟 1：建立用戶註冊 Command
我要建立：
- `RegisterUserCommand.cs` - 匿名用戶註冊命令
- `RegisterUserCommandHandler.cs` - 處理用戶註冊邏輯
- 包含設備指紋驗證、暱稱生成、防重複註冊

#### 步驟 2：建立用戶查詢 Query  
我要建立：
- `GetUserByDeviceFingerprintQuery.cs` - 根據設備指紋查找用戶
- `GetUserByIdQuery.cs` - 根據 ID 查找用戶
- `GetUserConnectionsQuery.cs` - 查詢用戶連線狀態

#### 步驟 3：建立用戶 Repository
我要建立：
- `IUserRepository.cs` 介面
- `UserRepository.cs` 實作 - 包含所有資料庫操作

### 第二階段：RESTful API Controller（預估 30-45 分鐘）

#### 步驟 4：建立 UserController
我要建立：
- `UserController.cs` - RESTful API 端點
- 端點包含：
  - `POST /api/users/register` - 註冊用戶
  - `GET /api/users/me` - 取得當前用戶資訊
  - `GET /api/users/{id}` - 取得指定用戶資訊
  - `PUT /api/users/me/nickname` - 更新暱稱

### 第三階段：整合 SignalR Hub（預估 1-2 小時）

#### 步驟 5：擴展 CommunicationHub
我要修改現有的 `CommunicationHub.cs`：
- 新增 `RegisterUser(existingUserId?, deviceFingerprint)` 方法
- 新增 `UpdateNickname(newNickname)` 方法
- 新增連線-用戶綁定機制
- 實作用戶狀態事件廣播

### 第四階段：服務註冊與測試（預估 30 分鐘）

#### 步驟 6：DI 服務註冊
我要修改：
- `Program.cs` - 註冊 Repository 和相關服務

#### 步驟 7：基礎測試驗證
我要建立簡單的端到端測試：
- 測試用戶註冊流程
- 測試 SignalR 用戶綁定
- 驗證設備指紋機制

## 🔧 技術細節

### 匿名用戶系統設計
- **用戶 ID**：使用 GUID 字串格式
- **設備指紋**：基於瀏覽器特徵生成唯一標識
- **預設暱稱**：「匿名用戶」+ 4位隨機數字
- **防重複**：同一設備指紋只能註冊一個用戶

### 資料流程
1. 前端生成設備指紋
2. 呼叫註冊 API 或 SignalR 方法
3. 後端驗證並建立用戶
4. 返回用戶資訊給前端
5. 建立 ConnectionId ↔ UserId 綁定

## ❓ 我的問題給你

**我現在要開始第一步：建立 RegisterUserCommand**

具體我會：
1. 建立 `RegisterUserCommand.cs` 文件，包含命令定義
2. 建立 `RegisterUserCommandHandler.cs` 文件，包含業務邏輯處理
3. 實作設備指紋驗證、用戶建立、暱稱生成等功能

**你同意嗎？要我開始第一步嗎？**

一旦你點頭，我就立即開始建立這些文件並實作功能。