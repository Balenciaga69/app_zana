---
applyTo: '**'
---
# User Feature V1

## 此 Feature 被 @Balenciaga69 拋棄，他要求重新構思

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

### Hub: CommunicationHub

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
---
# SignalR Hub Feature V1

## 此 Feature 被 @Balenciaga69 拋棄，他要求重新構思

## 概述

統一的 WebSocket 通訊中心，負責所有即時功能的協調和管理。採用單一 Hub 設計，避免多 Hub 間的複雜度。

## Hub: CommunicationHub

### 連線管理

#### 方法

- RegisterUser(existingUserId?, deviceFingerprint) - 用戶註冊/重新連線
- Heartbeat() - 心跳保持連線
- GetConnectionInfo() - 取得當前連線資訊

#### 事件

- UserRegistered(userId, nickname, isNewUser) - 用戶註冊成功
- ConnectionEstablished(connectionId, serverTime) - 連線建立確認
- ForceDisconnect(reason) - 強制斷線通知

### 房間管理

#### 方法

- JoinRoom(roomId, password?) - 加入房間
- LeaveRoom(roomId) - 離開房間
- GetRoomOnlineCount(roomId) - 取得房間在線人數

#### 事件

- RoomJoined(roomId, roomInfo, participantCount) - 成功加入房間
- RoomLeft(roomId) - 成功離開房間
- RoomMemberJoined(roomId, userId, nickname, joinedAt) - 有人加入房間
- RoomMemberLeft(roomId, userId, nickname, leftAt) - 有人離開房間
- RoomOnlineCountChanged(roomId, onlineCount) - 房間人數變更
- RoomDestroyed(roomId, reason) - 房間被銷毀通知
- RoomSettingsChanged(roomId, newSettings) - 房間設定變更
- JoinRoomFailed(reason) - 加入房間失敗

### 訊息管理

#### 方法

- SendMessageToRoom(roomId, content) - 發送訊息到房間

#### 事件

- ReceiveMessage(roomId, messageId, senderId, senderNickname, content, timestamp) - 接收新訊息
- MessageDelivered(messageId) - 訊息送達確認（可選）

### 用戶狀態

#### 方法

- UpdateNickname(newNickname) - 更新暱稱
- GetGlobalOnlineStats() - 取得全域在線統計

#### 事件

- NicknameUpdated(userId, newNickname) - 暱稱更新成功
- GlobalOnlineCountChanged(totalUsers, totalConnections) - 全域在線人數變更
- UserStatusChanged(userId, isOnline, lastActiveAt) - 用戶狀態變更

### 通用事件

- Error(errorMessage) - 錯誤通知
- ServerNotification(title, message, level) - 伺服器通知

## 權限與驗證

### 連線驗證

- 每個 WebSocket 操作都需驗證 ConnectionId 與 UserId 的綁定關係
- 未註冊的連線僅允許執行 RegisterUser 方法
- 超過 5 分鐘未註冊的連線會被自動斷開

### 房間權限

- 加入房間前需驗證密碼（如有設定）
- 發送訊息前需驗證用戶是否為房間成員
- 房主擁有房間設定修改權限

### 頻率限制

- 同一用戶每秒最多發送 3 則訊息
- 暱稱更新每分鐘最多 5 次
- 心跳頻率限制在每 10 秒一次

## 連線生命週期

### OnConnectedAsync

1. 記錄連線資訊（IP、UserAgent、ConnectionId）
2. 設定 5 分鐘註冊超時
3. 發送 ConnectionEstablished 事件

### OnDisconnectedAsync

1. 清理用戶連線記錄
2. 自動離開所有已加入的房間
3. 更新全域在線統計
4. 通知相關房間成員

### 重連機制

- SignalR 自動重連 + 自訂身份恢復
- 重連後自動恢復之前加入的房間
- 支援跨瀏覽器分頁的多連線管理

## 錯誤處理

### 常見錯誤碼

- AUTH_REQUIRED - 需要先註冊用戶
- ROOM_NOT_FOUND - 房間不存在
- ROOM_FULL - 房間人數已滿
- WRONG_PASSWORD - 房間密碼錯誤
- NOT_ROOM_MEMBER - 非房間成員
- RATE_LIMITED - 操作頻率過高
- INVALID_INPUT - 輸入參數無效

### 錯誤處理策略

- 所有異常都透過 Error 事件回報給客戶端
- 嚴重錯誤會觸發 ForceDisconnect
- 記錄詳細錯誤日誌供除錯分析

## 效能優化

### 群組管理

- 房間成員使用 SignalR Groups 管理
- 動態加入/離開群組，避免無效訊息傳送
- 支援大量並發連線的擴展性設計

### 快取策略

- 用戶連線狀態快取到 Redis
- 房間成員列表快取到 Redis
- 減少資料庫查詢頻率

### 訊息廣播優化

- 房間內訊息僅發送給該房間成員
- 避免全域廣播造成的效能問題
- 支援未來的水平擴展需求

## 監控與日誌

### 關鍵指標

- 即時連線數
- 訊息傳送成功率
- 平均連線持續時間
- 房間活躍度統計

### 日誌記錄

- 連線建立/斷開事件
- 用戶操作行為日誌
- 錯誤和異常情況記錄
- 效能相關指標追蹤

## 未來擴展

### 微服務準備

- Hub 方法設計考慮未來拆分需求
- 支援多個後端實例的負載均衡
- 使用 Redis Backplane 進行實例間通訊

### 功能擴展

- 語音/視訊通話信令支援
- 檔案傳輸進度通知
- 多媒體內容共享機制
- 自訂表情符號系統

