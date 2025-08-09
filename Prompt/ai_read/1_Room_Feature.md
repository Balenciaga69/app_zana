# Room Feature

## Entities

### Room（房間本體）

- Id: Guid
- Name: string
- OwnerId: string
- PasswordHash: string?
- MaxParticipants: int
- IsActive: bool
- LastActiveAt: DateTime
- InviteCode: string
- DestroyedAt: DateTime?
- CreatedAt: DateTime
- UpdatedAt: DateTime

### RoomParticipant（房間參與者/狀態）

- Id: Guid
- RoomId: Guid
- UserId: string
- JoinedAt: DateTime
- LeftAt: DateTime?（null 代表目前在線）
- CreatedAt: DateTime
- UpdatedAt: DateTime

## RESTful API

### 房間管理（CRUD）

- 建立房間 POST /rooms
- 取得房間資訊 GET /rooms/{id}
- 更新房間設定 PUT /rooms/{id}（僅房主）
- 取得房間統計 GET /rooms/{id}/stats

### 查詢功能

- 透過 InviteCode 查詢房間 GET /rooms/invite/{inviteCode}
- 取得房間參與者列表 GET /rooms/{id}/participants
- 取得我參與的房間列表 GET /users/me/rooms

### 房間連結分享

- 產生分享連結 GET /rooms/{id}/share-link
- 驗證房間密碼 POST /rooms/{id}/verify-password

## WebSocket (SignalR) API

### Hub: ChatHub

#### 房間操作方法

- **JoinRoom(roomId, password?)** - 加入房間
- **LeaveRoom(roomId)** - 離開房間
- **GetRoomOnlineCount(roomId)** - 取得房間在線人數

#### 客戶端接收事件

- **RoomJoined(roomId, roomInfo, participantCount)** - 成功加入房間
- **RoomLeft(roomId)** - 成功離開房間
- **RoomMemberJoined(roomId, userId, nickname, joinedAt)** - 有人加入房間
- **RoomMemberLeft(roomId, userId, nickname, leftAt)** - 有人離開房間
- **RoomOnlineCountChanged(roomId, onlineCount)** - 房間人數變更
- **RoomDestroyed(roomId, reason)** - 房間被銷毀通知
- **RoomSettingsChanged(roomId, newSettings)** - 房間設定變更（房主操作）
- **JoinRoomFailed(reason)** - 加入房間失敗
- **Error(errorMessage)** - 錯誤通知

### 即時通訊流程

1. 用戶透過 SignalR 加入房間 → 驗證密碼和人數限制
2. 加入成功後即時通知房間內所有成員
3. 用戶離開房間時即時通知其他成員
4. 房間狀態變更（如設定修改）即時廣播

## 背景服務

### 房間自動清理服務

- **空房間檢測**：每 10 分鐘檢查一次無成員的房間
- **自動銷毀條件**：房間無成員且超過 30 分鐘未活動
- **銷毀通知**：透過 SignalR 通知最後離開的用戶（如仍在線）

## 設計備註

### 權限控制

- **房主權限**：修改房間名稱、密碼、人數限制
- **成員權限**：查看房間資訊、發送訊息、離開房間
- **訪客權限**：僅能透過 InviteCode 查詢房間基本資訊

### 房間狀態管理

- **人數限制**：加入時檢查是否超過 MaxParticipants
- **密碼保護**：密碼使用 bcrypt 雜湊儲存
- **活躍追蹤**：有訊息發送或成員異動時更新 LastActiveAt
- **狀態一致性**：RESTful API 與 WebSocket 的權限驗證邏輯保持一致

### 效能考量

- **快取策略**：房間基本資訊和在線人數使用 Redis 快取
- **資料庫索引**：InviteCode、OwnerId、IsActive 建立索引
- **即時性優先**：進出房間使用 WebSocket，查詢歷史使用 RESTful

### 未來擴展

- QRCode 生成功能（透過 share-link 端點）
- 房間主題或標籤系統
- 房間活躍度統計分析
