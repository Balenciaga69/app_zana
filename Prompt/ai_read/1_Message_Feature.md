# Message Feature

## Entities

### Message（訊息本體）

- Id: Guid
- RoomId: Guid
- SenderId: string
- Content: string
- CreatedAt: DateTime
- UpdatedAt: DateTime

## RESTful API

### 歷史訊息查詢（只讀操作）

- 查詢房間歷史訊息 GET /rooms/{roomId}/messages?skip=0&take=50&before={timestamp}
- 查詢單一訊息 GET /messages/{id}

### 備註

- 發送訊息**不使用** RESTful API，改用 WebSocket (SignalR)
- 歷史訊息查詢支援分頁、時間過濾、按時間排序
- 查詢時需驗證用戶是否為該房間成員

## WebSocket (SignalR) API

### Hub: ChatHub

#### 訊息相關方法

- **SendMessageToRoom(roomId, content)** - 發送訊息到指定房間
- **JoinRoom(roomId, password?)** - 加入房間（訊息接收前提）
- **LeaveRoom(roomId)** - 離開房間

#### 客戶端接收事件

- **ReceiveMessage(roomId, messageId, senderId, senderNickname, content, timestamp)** - 接收新訊息
- **MessageDelivered(messageId)** - 訊息送達確認（可選）
- **RoomMemberJoined(roomId, userId, nickname)** - 有人加入房間
- **RoomMemberLeft(roomId, userId, nickname)** - 有人離開房間
- **Error(errorMessage)** - 錯誤通知

### 即時通訊流程

1. 用戶透過 SignalR 發送訊息 → 後端驗證房間成員權限
2. 訊息先持久化到資料庫 → 取得 MessageId 和時間戳
3. 廣播給房間內所有在線成員 → 包含完整訊息資訊
4. 可選：送達確認回報

## 設計備註

- **訊息不可編輯、刪除、修改**（永久保存）
- **訊息內容僅支援純文字**，最大長度限制 2000 字元
- **發送前驗證**：用戶必須為該房間成員且房間處於活躍狀態
- **防濫用機制**：同一用戶每秒最多發送 3 則訊息
- **訊息持久化優先**：先存 DB 再廣播，確保資料一致性
- **資料庫索引**：(RoomId, CreatedAt) 複合索引，支援快速歷史查詢
- **即時性優先**：WebSocket 比 HTTP 更適合高頻訊息交換