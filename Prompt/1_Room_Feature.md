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

## Restful API

### CRUD

- 建立房間 POST/rooms
- 取得房間 GET/rooms/{id}
- 更新房間 PUT/rooms/{id}

### 查詢參與者

- 取得房間 InviteCode GET/rooms/invite/{inviteCode}
- 取得房間參與者們 GET/rooms/{id}/participants

### 用戶進出房間

- 用戶加入房間 POST/rooms/{id}/join
- 用戶離開房間 POST/rooms/{id}/leave

### 備註

- 刪除房間是系統自動處理的，當房間沒有成員且超過一定時間未活動時，系統會自動刪除。
- 用戶斷線會被系統處理為離開所有房間。
