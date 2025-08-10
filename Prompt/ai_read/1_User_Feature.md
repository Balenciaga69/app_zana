# User Feature

## Entities

### User（用戶本體）

- Id: Guid
- IsActive: bool
- LastActiveAt: DateTime
- DeviceFingerprint: string
- Nickname: string
- CreatedAt: DateTime
- UpdatedAt: DateTime

### UserConnection（用戶連線）

- Id: Guid
- UserId: string
- ConnectionId: string（SignalR 連線 Id）
- ConnectedAt: DateTime
- DisconnectedAt: DateTime?（null 代表目前在線）
- IpAddress: string?
- UserAgent: string?
- CreatedAt: DateTime
- UpdatedAt: DateTime

## RESTful API

### 用戶資料管理

- 取得用戶資訊 GET /users/{id}
- 取得我的資訊 GET /users/me
- 更新用戶暱稱 PUT /users/me/nickname
- 取得我的連線歷史 GET /users/me/connections

### 狀態查詢

- 查詢用戶是否在線 GET /users/{id}/is-online
- 查詢全域在線統計 GET /users/online-stats
- 查詢用戶參與的房間 GET /users/{id}/rooms

### 設備指紋管理

- 註冊新設備 POST /users/device-register
- 驗證設備指紋 POST /users/device-verify

## WebSocket (SignalR) API

### Hub: ChatHub

#### 連線管理方法

- RegisterUser(existingUserId?, deviceFingerprint) - 註冊/重新連線用戶
- UpdateNickname(newNickname) - 即時更新暱稱
- GetMyConnectionInfo() - 取得當前連線資訊
- Heartbeat() - 心跳保持連線

#### 客戶端接收事件

- UserRegistered(userId, nickname, isNewUser) - 用戶註冊成功
- ConnectionEstablished(connectionId, serverTime) - 連線建立確認
- NicknameUpdated(userId, newNickname) - 暱稱更新成功
- GlobalOnlineCountChanged(totalUsers, totalConnections) - 全域在線人數變更
- UserStatusChanged(userId, isOnline, lastActiveAt) - 用戶狀態變更
- ForceDisconnect(reason) - 強制斷線（如重複連線）
- Error(errorMessage) - 錯誤通知

### 即時通訊流程

1. 初次連線：用戶提供 deviceFingerprint → 系統產生 userId 或識別既有用戶
2. 重新連線：用戶提供 existingUserId + deviceFingerprint → 驗證後恢復身份
3. 狀態同步：用戶上下線即時更新全域統計
4. 跨房間身份：同一 userId 可在多個房間間保持身份一致性

## 背景服務

### 用戶清理服務

- 非活躍用戶檢測：每小時檢查超過 7 天未活動的用戶
- 連線清理：定期清理已斷線超過 24 小時的 UserConnection 記錄
- 設備指紋重複檢測：防止同一設備產生多個用戶身份

### 連線監控服務

- 心跳檢測：每 30 秒檢查用戶連線狀態
- 殭屍連線清理：清理無回應的 SignalR 連線
- 異常斷線處理：自動標記斷線用戶為離線狀態

## 設計備註

### 匿名用戶系統

- 無需註冊：系統自動分配 userId（GUID 字串格式）
- 設備綁定：同一裝置同一瀏覽器視為同一用戶（依 DeviceFingerprint）
- 暱稱系統：可選填，預設為「匿名用戶」+ 隨機編號
- 隱私保護：不收集個人識別資訊，僅保留技術必要資料

### 設備指紋技術

- 指紋生成：基於瀏覽器特徵、螢幕解析度、時區等生成唯一標識
- 指紋驗證：連線時驗證 deviceFingerprint 與 userId 的綁定關係
- 防偽造：結合多重瀏覽器特徵，提高指紋唯一性和穩定性

### 連線狀態管理

- 多連線支援：同一用戶可在多個分頁或裝置建立連線
- 斷線重連：SignalR 自動重連機制 + 自訂身份恢復邏輯
- 優雅降級：連線異常時，用戶狀態會自動標記為離線

### 權限與安全

- 用戶隔離：每個用戶僅能存取自己的資料
- 連線驗證：每次 WebSocket 操作都驗證 connectionId 與 userId 的關聯
- 頻率限制：防止暱稱更新、重複註冊等操作被濫用

### 效能考量

- Redis 快取：用戶在線狀態、連線資訊使用 Redis 儲存
- 資料庫索引：DeviceFingerprint、LastActiveAt 建立索引
- 分頁查詢：連線歷史等大量資料支援分頁載入

### 監控與統計

- 實時統計：全域在線用戶數、總連線數
- 活躍度追蹤：用戶最後活動時間、房間參與情況
- 連線品質：平均連線時長、斷線重連頻率等指標
