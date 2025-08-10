# SignalR Hub Feature

## 概述

統一的 WebSocket 通訊中心，負責所有即時功能的協調和管理。採用單一 Hub 設計，避免多 Hub 間的複雜度。

## Hub: ChatHub

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
