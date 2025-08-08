# Message Feature

## Entities

### Message（訊息本體）

- Id: Guid
- RoomId: Guid
- SenderId: string
- Content: string
- CreatedAt: DateTime
- UpdatedAt: DateTime

## Restful API

### CRUD

- 發送訊息 POST /messages
- 查詢房間歷史訊息 GET /rooms/{roomId}/messages
- 查詢單一訊息 GET /messages/{id}
- 刪除訊息 DELETE /messages/{id}（可選，通常只允許自己刪除）

### 即時通訊

- 透過 SignalR 廣播新訊息
- 支援訊息送達回報（可選）

## 設計備註

- 訊息不可編輯（如需可再補充 PATCH/PUT）
- 訊息內容僅支援純文字
- 訊息發送時需驗證用戶是否為該房間成員
- 歷史訊息查詢支援分頁（如：`?skip=0&take=50`）
- 訊息資料庫表需有索引（RoomId, CreatedAt）

## 事件與流程

- 用戶發送訊息 → API 儲存訊息 → SignalR 廣播給房間所有在線成員
- 用戶進房時查詢歷史訊息