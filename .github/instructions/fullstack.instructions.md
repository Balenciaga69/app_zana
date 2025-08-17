---
applyTo: '**'
---
# GitHub Copilot Instruction：匿名即時聊天室
### @Copilot Agent 模式須知
- 開發環境：Windows + VSCode + Docker（勿誤用 Linux 語法）
- 前後端溝通用: Restful API and SignalR
- 不用自動編譯與測試，如有誤會主動告知
- 每次大幅修改前需說明：
  - 為什麼要改
  - 會改動/新增/刪除哪些檔案
  - 會怎麼改
  - 預期結果
---
### @Balenciaga69 的說明
- API 為了前端需要才開，遇到了再開發，而不是事先規劃好所有 API
- 有遇到了再來煩惱，而不是一開始就設計好所有細節
- 如果計劃書有標記被棄用或需修改，請直接忽略後重新構思
---
### 核心理念
- 無會員制：免登入、匿名、純文字聊天。
- 即時通訊與分房：支援即時訊息傳輸，使用者可自由進出房間。
- 穩固與可擴展：具備可監控與持久化，並為未來擴展與微服務化預留彈性。
---
## 後端專屬
### 必備技術棧
- **核心框架**：C# .NET 8+
- **即時通訊**：SignalR
- **資料庫**：PostgreSQL + EF Core
- **快取/暫存**：Redis
- **日誌**：Serilog
- **CQRS**：MediatR
- **容器化**：Docker
- **測試**：xUnit、Moq、FluentAssertions
- **CI/CD**：GitHub Actions
- **API 文件**：Swagger
### 架構模式
- 架構類型：單體（未來可轉微服務）
- 設計模式：
  - 命令/查詢模式（MediatR）
  - Repository Pattern（介面與實作同檔案）
- 程式碼組織：
  - 依 Feature 切分（Commands、Queries、Controller、Repositories）
  - `DbContext`、`Entities` → `Infrastructure`
  - 通用元件 → `Shared`
  - Controller 保持輕量，只連接 Command/Query
  - CommunicationHub 作為單一 Real-time Hub
  - 跨微服務/Feature 通訊 → RabbitMQ/MassTransit
### 資料庫設計原則
- 僅保留 ID 引用，不使用 Navigation Property
- 所有 Entity 需有 `CreatedAt`、`UpdatedAt`、`Id`
### 日誌紀錄規範
- 一般業務、API、Command/Query → MediatR Pipeline 自動紀錄
- 錯誤/例外 → Middleware 處理
- RabbitMQ 消費、SignalR Hub → 僅在業務關鍵時補充 log
- 禁止重複紀錄一般流程 log
- 格式、TraceId、結構化 → 統一用 IAppLogger
### 未來規劃（暫不實作）
- 不導入：K8s、AWS、Grafana、Prometheus、ELK、YARP、微服務拆分
- 不支援功能：影音、會員、好友制、檔案傳輸、QRCode、語音合成
- 不採用：DDD、乾淨架構、CQRS（完整）、JWT、TDD、AutoMapper
---
## 前端專屬
### 必備技術棧
- **框架**：React + TypeScript
- **開發/打包**：Vite
- **UI**：Chakra UI（原子設計系統）
- **狀態管理**：Zustand
- **API 請求**：Axios
- **即時通訊**：SignalR Client
- **路由**：React Router Dom
- **程式碼品質**：ESLint、Prettier
### 架構與設計原則
- 元件化：單一職責，每個 React 元件獨立
- 原子設計：Atom → Molecule → Organism
- 狀態管理：UI 與業務邏輯分離
- 樣式：以 Chakra UI 為主，局部 `sx` 或 `styled`
- API 處理集中管理，與 UI 解耦
- Hooks 謹慎使用（`useEffect`, `useCallback`, `useMemo`）
- 路由集中配置
### 已取消或未來規劃
- 初期不考慮影音、會員、好友制、檔案傳輸
---
## 通用
### 功能需求
- **房間管理**：建立、加入、離開房間；可設定密碼、名稱、人數上限；無人房間自動銷毀
- **使用者行為**：發送/接收訊息，支援歷史訊息查詢
- **連線與識別**：支援斷線重連，同瀏覽器視為同用戶，同用戶可同時進入多個房間
- **資訊顯示**：統計房間在線人數，提供房間連結分享
### 已完成基礎設施
- 健康檢查（PostgreSQL, Redis）
- EF Core DbContext、Entities 與配置
- Migration → PostgreSQL
- 通用 API Response、Exception Filter、日誌
- 輕量 Controller
- CommunicationHub 作為單一 Real-time Hub
- 跨微服務/Feature 通訊 → RabbitMQ/MassTransit
---
