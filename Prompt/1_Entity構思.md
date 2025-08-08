#### 註：
所有 Entities 都包含 CreatedAt、UpdatedAt、Id

## Room Feature

### Room（房間本體）
- Id: Guid
- Name: string
- OwnerId: string
- PasswordHash: string?
- MaxParticipants: int
- IsActive: bool
- LastActiveAt: DateTime
- DestroyedAt: DateTime?

### RoomParticipant（房間參與者/狀態）
- Id: Guid
- RoomId: Guid
- UserId: string
- JoinedAt: DateTime
- LeftAt: DateTime?（null 代表目前在線）

---

## Message Feature

### Message（訊息本體）
- Id: Guid
- RoomId: Guid
- SenderId: string
- Content: string
- CreatedAt: DateTime

---

## User Feature

### User（用戶本體）
- Id: string
- IsActive: bool
- LastActiveAt: DateTime
- DeviceFingerprint: string
- Nickname: string?

### UserConnection（用戶連線）
- Id: Guid
- UserId: string
- ConnectionId: string（SignalR 連線 Id）
- ConnectedAt: DateTime
- DisconnectedAt: DateTime?（null 代表目前在線）
- IpAddress: string?
- UserAgent: string?

---

#### 補充說明：
- RoomConfig 已合併進 Room，避免重複與查詢困難。
- RoomParticipant 與 UserRoomSession 合併，統一記錄進出歷史與當前在線。
- Message 明確列出 CreatedAt。
- User 增加 Nickname 以利訊息辨識（可選）。
- 所有 Id 欄位皆明確標示。
- 其餘欄位依需求補齊，無冗贅、無衝突。

Entity 你來想想看 這一套是否有冗贅沒必要存在的欄位 或者衝突打架了 或者缺失了甚麼無法完成某些功能?

