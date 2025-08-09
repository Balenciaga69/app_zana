### 溝通原則:

- 繁體中文交流
- 高度專業，不需要過多解釋性交談
- 偏好逐步型最小完成步驟，而非一次大而美搞定。

---

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
- 房間沒人要自動銷毀、對話也跟著不見。
- 房間建立與加入：
  - 任何使用者都可以建立一個新的聊天室。
  - 使用者僅能透過特定的房間網址加入聊天室，不提供房間發現功能。

#### 開發階段: 真正有趣的功能擴展(Paul)

- 整合遊戲模板: 新增如「狼人殺」等遊戲模板，豐富聊天室的互動性。
- 導入 AI 模型: 整合 OpenAI 或 Gemini API，為聊天室帶來更智慧的互動體驗。

#### 開發階段: 成熟的進階功能(Anderson)

- 訊息內容審查 & 惡意內容過濾
- 防灌水 & 洗版面

---

### 技術選型與架構

#### 必要技術棧

- 前端: React,Typescript,Vite,ChakraUI,Zustand,SignalR Client,React Hook Form,React Router Dom,Axios
- 後端: C# .NET 8+, SignalR, gRPC, EF Core, PostgreSQL, Redis, RabbitMQ, YARP, MediatR, AutoMapper, Polly, FluentValidation, Serilog, MassTransit
- DevOps: Git, Docker, Health Checks, GitHub Actions
- 測試: xUnit, Moq, FluentAssertions
- 輔助工具: Eslint, Prettier,Swagger, Postman
- 其他工具: ChatGPT, Github Copilot, Gemini API/OpenAi API
- 開發概念: 測試驅動(CRUD 不用)、事件驅動(少部分)、儲存庫模式(Repository Pattern)

#### 後期可選用技術棧(後端篇)

- Prometheus
- Grafana
- ELK Stack (Elasticsearch, Logstash, Kibana)
- Kubernetes (K8s)
- AWS
- 後端橫切面功能: Logging, Configuration, Error Handling, Health Checks, 驗證, JSON 序列化, 指標蒐集, 訊息代理程式互動等等
- 具有規模後可考慮: 乾淨架構、領域驅動、事件溯源、讀寫分離

#### 橫切面功能盤點(後端篇)

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

- 初期架構：從單體架構開始（Controller/Service/Models），並預留 MediatR、AutoMapper、FluentValidation、Serilog、MassTransit 等橫切面功能的整合點。
- 後端結構：初期採用 Feature Folder 結構，避免過早拆分導致複雜度。
- 逐步開發：先專注於各服務的核心功能與獨立運行，再導入 RabbitMQ、gRPC 等通訊機制。
- 簡化前期：延後雲端部署（K8s）與完整 CI/CD 流程，以利專注於核心功能開發。
- 功能聚焦：MVP 階段不考慮登入、金流、好友、會員等功能。
- 專案性質：此為個人專案，無團隊協作。
- 上雲時機：待 MVP 在地端驗證可行後再上雲。
- 部署環境：優先以地端分散式架構進行開發。
- 專案結構：初期不拆分多個 csproj，僅透過資料夾與命名空間區隔服務。
- 通用模組：非必要不預先開發 BuildingBlock；僅當多個模組需要時才建立，或於 MVP 結束後再整理。
- 低耦合：確保模組間鬆散耦合，例如房間模組不應處理登入或金流相關邏輯。

---

## Jackson 階段 (前端)：

- 包含:
  - 房間首頁
  - 建立房間頁
  - 加入房間頁
  - 聊天室頁
- 使用 Chakra UI 進行樣式設計
- 使用 Zustand 管理全域狀態
- 使用 React Router 進行路由管理
- 使用 React Hook Form 處理表單
- 使用 Axios 進行 API 請求
- 使用 SignalR Client 進行即時通訊
