# User Feature

## Entities

### User（用戶本體）

- Id: string
- IsActive: bool
- LastActiveAt: DateTime
- DeviceFingerprint: string
- Nickname: string?
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

## Restful API

### CRUD

- 取得用戶 GET/users/{id}
- 取得我自己 GET/users/me
- 更新用戶暱稱 PUT/users/{id}/nickname

### 狀態查詢

- 查詢用戶是否在線 GET/users/{id}/is-online
- 查詢所有在線用戶 GET/users/online

### 備註

- 用戶資料由系統自動建立，無需註冊。
- 同一裝置同一瀏覽器視為同一用戶（依 DeviceFingerprint）。
- 用戶斷線時，所有連線會自動標記為離線。
- 暱稱可選填，預設為匿名。


