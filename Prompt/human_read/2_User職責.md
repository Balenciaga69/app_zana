GitHub Copilot

簡短回答（MVP 取捨方向）：

1) User 本質上該做甚麼？
- 單純代表使用者身份（匿名或簡易暱稱），維護識別與在線狀態。  
- 必要欄位（MVP）：Id (GUID), Nickname, DeviceFingerprint, LastActiveAt, IsActive, CreatedAt, UpdatedAt。  
- 延伸但非必需：多連線記錄（UserConnection）與 IP/UserAgent（用於診斷、異常偵測）。

2) 前端何時會跟 User 交互（最常見場景）？
- App/頁面 開啟：發送 deviceFingerprint（或 localStorage 的 userId）向後端註冊／恢復身份。  
- 建立 WebSocket(SignalR) 連線：註冊使用者 => server 回傳 userId、nickname。  
- 顯示個人資料頁面：讀取 /users/me。  
- 更新暱稱：使用者在 UI 改暱稱時呼叫 PUT /users/me/nickname 或 Hub.UpdateNickname。  
- 顯示在線狀態（朋友/房間名單）：需要查詢 /users/{id}/is-online 或由 SignalR 推播 UserStatusChanged。  
- 重連/多分頁：當 SignalR 重連或新分頁啟動時，前端送 deviceFingerprint (+ 已有 userId) 以恢復身份。

3) 最低限度能運行的 API / Hub（MVP）
- REST（最小集合）：
  - GET /users/me -> 回傳 Id, Nickname, IsActive, LastActiveAt
  - PUT /users/me/nickname { "nickname": "..." } -> 更新暱稱
  - POST /users/device-register { "deviceFingerprint": "..." } -> 可選：建立/綁定 device -> 回 userId（或可只用 SignalR 做此步）

- SignalR Hub（最小集合）：
  - 方法：RegisterUser(existingUserId?, deviceFingerprint) -> 回傳 { userId, nickname, isNewUser }  
  - 方法：Heartbeat() -> 更新 LastActiveAt（或由 SignalR 自動記錄）  
  - 方法：UpdateNickname(newNickname) -> 廣播 NicknameUpdated
  - 事件（Server -> Client）：UserRegistered, ConnectionEstablished, NicknameUpdated, UserStatusChanged, Error

4) 最小工作流程（前端視角）
- 啟動：讀 localStorage.userId & deviceFingerprint。
  - 若無 userId -> 連 SignalR 呼 RegisterUser(deviceFingerprint)；儲存回傳 userId。
  - 若有 userId -> 呼 RegisterUser(existingUserId, deviceFingerprint) 以嘗試恢復。
- 連線成功後：顯示 nickname、訂閱狀態事件、每 30s 呼 Heartbeat（或由 server 依 Connection 活動自動更新）。
- 改暱稱：呼 UpdateNickname -> 等 Hub 回應，再更新本地 UI。

5) 簡單資料契約範例
- RegisterUser 請求: { "existingUserId": "guid?", "deviceFingerprint": "string" }  
- RegisterUser 回應: { "userId":"guid","nickname":"匿名1234","isNewUser": true }

6) 實作建議（MVP 重點）
- 將「身份建立/恢復」邏輯集中在 SignalR Hub（首連即建立 user），REST 只做查詢/少量修改。  
- 用一個簡單 UserConnection table 記錄 connectionId、ConnectedAt、DisconnectedAt（方便查 is-online）。  
- 可暫用記憶體/Redis 快取 online 狀態（避免每次查 DB）。  
- 頻率限制先用粗略策略：Heartbeat 每 10–30 秒一次，暱稱更新每分鐘 ≤5 次，訊息流量另控。

7) 移除的複雜功能（非 MVP）
- 複雜的設備指紋防偽、跨設備複雜同步、多重用戶合併、自動清理多級背景任務（可後續加）。

結論（一句話）：MVP 聚焦「匿名身份建立/恢復 + 基本在線狀態 + 暱稱更新」；把大部分即時需求放在 SignalR（RegisterUser、Heartbeat、UpdateNickname）上，REST 只提供查詢與少量修改介面，這樣前端的整合最簡潔且足夠運作。需要我把上述最小 API 與 Hub 介面寫成 swagger / 接口草案或範例程式碼嗎？