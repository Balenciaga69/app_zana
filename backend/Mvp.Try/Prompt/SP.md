### 專案概述與 MVP 定義

- 本專案旨在建立一個基於 chatGPT/Gemini API 的多人共同討論聊天室。
- 使用者無需登入即可匿名參與，透過回合制協作與 AI 共同完成內容。
- MVP 階段將嚴格聚焦於核心後端服務的建立、微服務間的通訊驗證、以及在地端環境的完整 DevOps 流程驗證。
- AI 相關功能將優先以 Mock 方式實現，確保其他系統組件正常運行。

### 開發階段順序與代號

#### 開發階段: 原始純粹聊天室(Jackson)

- 有房間、密碼功能、人數上限功能
- 同一瀏覽器上斷線重連要找回房間
- 能存取資料庫
- 能承受奧克蘭、台北、紐西蘭朋友們共計可能同時 100 人使用在線(超過 100 位時候我會直接拒絕他建立任何連線加入任何房間)
- 能監控當前在線人數(同時連線數)、伺服器整體狀況
- 房間設定選項: 房間建立者可設定人數上限及進入密碼，以控制房間的存取。
- 純文字聊天功能: 聊天室內僅支援純文字訊息交流。
- 完全匿名，無需註冊登入: 使用者無需建立帳號或登入，即可直接使用。
- 彈性房間參與人數: 每個聊天室可容納 1 至 10 人。
- 無好友、無匹配機制: 類似 WooTalk 的模式，使用者不會有好友列表，也不會進行自動配對。
- 房間建立與加入：
  - 任何使用者都可以建立一個新的聊天室。
  - 使用者僅能透過特定的房間網址加入聊天室，不提供房間發現功能。

#### 開發階段: 真正有趣的功能擴展(Paul)

- 整合遊戲模板: 新增如「狼人殺」等遊戲模板，豐富聊天室的互動性。
- 導入 AI 模型: 整合 OpenAI 或 Gemini API，為聊天室帶來更智慧的互動體驗。

#### 開發階段: 成熟的進階功能(Anderson)

- 訊息內容審查 & 惡意內容過濾
- 防灌水 & 洗版面

### 必要技術棧

- 前端: React,Typescript,Vite,ChakraUI,Zustand,SignalR Client,React Hook Form,React Router Dom,Axios
- 後端: C# .NET 8+, SignalR, gRPC, EF Core, PostgreSQL, Redis, RabbitMQ, YARP, MediatR, AutoMapper, Polly, FluentValidation, Serilog, MassTransit
- DevOps: Git, Docker, Health Checks, GitHub Actions
- 測試: xUnit, Moq, FluentAssertions
- 其他工具: Swagger, Postman, ChatGPT, Github Copilot, Gemini API/OpenAi API

### 後期選用技術棧

- Prometheus
- Grafana
- ELK Stack (Elasticsearch, Logstash, Kibana)
- Kubernetes (K8s)
- AWS

---

### 後端橫切面功能盤點

- Logging : Serilog + JSON 輸出，成熟可用後需要推到 ELK 上
- Configuration : 使用統一配置系統，MVP 階段交給 Docker-compose 管理
- ErrorHandle 跟 Exception : 需要搭配統一的 Pipe,Middleware,Filter
- HealthCheck
- 驗證
- JSON 與其他資料序列化
- 指標蒐集 Prometheus Exporter + Grafana
- 訊息代理程式互動 : MassTransit

---

### 開發策略與注意事項

- 逐步遞增複雜度: 先專注於每個服務的核心功能和其獨立運行，再引入其間的通訊機制 (RabbitMQ, gRPC)。
- 減少前期阻礙: 將雲端部署 (K8s) 和完整的 CI/CD 流程延後，避免初期被複雜的基礎設施配置卡住，專注於功能開發。一人開發最忌諱一開始就鋪設過於龐大的基建，導致遲遲無法看到成果。
- 不考慮登入、金流、好友、會員等機制: 目前階段完全不考慮這些功能，未來有必要時再添加。
- 單打獨鬥專案: 這是個人專案，無團隊協作。
- 上雲時機: 待 MVP 在地端驗證完畢並確定可行後，再集中進行上雲部署工作。
- 地端分散式架構優先: 目前仍以地端、分散式架構為前提進行開發。
- 不拆分多個 csproj (初期): 初期不考慮將每個服務再拆分為 Domain, Application, Infrastructure 等多個 csproj，以簡化開發負擔，僅用資料夾與命名空間區隔。
- 不再開局就開發 BuildingBlock，除非多個 Module 都需要用才轉入 BuildingBlock，剩餘直到 MVP 結束收尾才整理成 BuildingBlock
- 不要過度緊密，以便未來好處理。例如房間模組不該驗證登入，AI 內容模組不該知道是否使用者有金流等無關自身服務現象。

---

# Jackson 階段(目前)

- 目標回顧
  - 此階段核心目標是建立一個免登入、無帳號機制、具備房間、密碼、人數上限、匿名聊天、斷線重連(需要保留聊天記錄)、資料庫存取能力，且能承受 100 人同時在線的聊天室服務。

## 個人目前預計開發步驟
1. 實驗 Websocket 功能前後端串接是否可行 (已經搭建前後端，實驗成功)
2. 實驗 UserId 等建立連線功能
   - UserId 設計建議
     - 建議用 GUID 或類似唯一識別碼，並存在前端 localStorage，斷線重連時自動恢復身份。
     - 後端收到新連線時，若沒帶 UserId，則產生一個新的回傳給前端。
   - 連線狀態管理
     - 可以先設計一個 Connection 物件，記錄 UserId、ConnectionId、狀態、IP 等，方便後續擴充。
   - 測試流程
     - 先用最簡單的方式（例如 SignalR Hub context）驗證 UserId 能正確分配、傳遞與查詢。
     - 可以先不考慮房間，單純驗證連線與 UserId 綁定。
   - 日誌與除錯
     - 建議在這階段就加上簡單的日誌，方便追蹤每個連線的 UserId 流程。
3. 實驗 創建房間能連線、能聊天功能
---
## 服務領域區分

### Room 負責房間的生命週期管理、狀態維護及存取控制。

- 主要功能:建立房間／加入房間／離開房間／房間狀態／銷毀房間
- 領域資料(暫定):
  - RoomId,OwnerUserId,RoomName,RoomPassword,MaxParticipants,
    Participants(Dictionary<UserId, RoomParticipantInfo>),
    IsLocked,CreatedAt,LastActivityAt
  - RoomParticipantInfo: UserId,JoinAt,LastActivityAt,DisplayName,AvatarUrl,IsInRoom
- 會有兩張表存房間與房間參與者

### Chat 負責處理即時訊息的傳輸、廣播及基本儲存。

- 主要功能: Message Sending／BroadCast,Storage
- 領域資料(暫定):
  - MessageId,RoomId,SenderUserId,Content,SentAt

### Connection 負責管理所有使用者 WebSocket 連接的生命週期與狀態。

- 領域規劃
  - ConnectionId(每次連線唯一 Id),UserId(跟瀏覽器綁定存在 LocalStorage),
    JoinedRoomIds(HasSet<RoomId>),ConnectedAt,LastActivityAt,IsConnected,ClientIpAddress

### API Gateway

- 統一外部請求入口，處理路由、負載均衡、身份驗證 (未來擴展) 等。
