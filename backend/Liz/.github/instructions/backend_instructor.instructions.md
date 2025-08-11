## 後端專案計畫書：匿名即時聊天室

### 核心理念

* 無會員制：免登入、匿名、純文字聊天。
* 即時通訊：支援即時訊息傳輸與分房機制。
* 可擴展性：可監控、可持久化，並具備未來微服務化的潛力。

---

### 技術與架構

#### 必備技術棧

這個專案將採用 C# .NET 8+ 作為核心框架，並利用 SignalR 實現即時通訊。資料庫部分，主資料庫選用 PostgreSQL 搭配 EF Core 進行物件關聯對應，同時以 Redis 處理快取與暫存資料。日誌紀錄會使用 Serilog，命令/查詢模式則由 MediatR 負責。專案將全程使用 Docker 進行容器化，並內建 Health Checks 確保服務健康。在品質把關方面，我們將以 xUnit 搭配 Moq 和 FluentAssertions 撰寫單元測試。版本控制則採用 Git，並利用 GitHub Actions 實現自動化工作流程。最後，Swagger 將用來產生與維護 API 文件。

#### 架構模式

* 架構類型：單體架構 (未來會轉為微服務)
* 設計模式：
    * 命令/查詢模式：使用 MediatR
    * 資料存取：Repository Pattern (介面與實作放在同一個檔案)
* 程式碼組織：
    * 依據 Features 切分，而非傳統的分層 (e.g., Controller, Service)
    * 每個 Feature 資料夾包含：`Commands` (含 Handler)、`Queries` (含 Handler)、`Controller`、`Repositories`
    * `DbContext` 與 `Entities` 集中放在 `Infrastructure`
    * 通用元件可放置於 `Shared`

#### 資料庫設計原則

* 避免直接外鍵關聯：只保留 ID 引用，不使用 Navigation Property。
* 通用欄位：所有 Entity 必須包含 `CreatedAt`、`UpdatedAt`、`Id`。

---

### 目前階段功能需求

* 房間管理
    * 使用者可建立房間、加入房間、離開房間。
    * 房間可設定密碼、名稱、人數上限。
    * 系統自動銷毀無人的空房間。
* 使用者行為
    * 使用者可發送與接收訊息。
    * 支援歷史訊息查詢。
* 連線與識別
    * 支援斷線後重連。
    * 同瀏覽器視為同一用戶。
    * 同一用戶可同時進入多個房間。
* 資訊顯示
    * 統計並顯示房間內的在線人數。
    * 提供房間連結分享功能。

---

### 已具備基礎架構

* 已建置健康檢查 (PostgreSQL, Redis)。
* 已完成 EF Core DbContext、Entities 與各種配置。
* 已可執行 Migration 至 PostgreSQL。
* 已具備通用 API Response、Exception Filter 與日誌紀錄功能。
* Controller 只負責連接 Command/Query，保持輕量化。
* 用 CommunicationHub 作為單一 Real-time Hub
* 跨微服務/Feature 通訊會使用 RabbitMQ/MassTransit
---

### @Copilot Agent 模式須知

* 開發環境：Windows + VSCode + Docker (別誤用Linux語法)
* 不用自動編譯與測試，如果有誤我會主動告知你
* 每次要大動刀之前要告知以下內容
    * 你為什麼要改
    * 你會改變哪些檔案或新增刪除哪些檔案
    * 你會怎麼改
    * 修改後預期結果是

---

### 未來規劃 (暫不實作)

專案初期暫不導入 K8s 和 AWS 進行部署，
監控工具如 Grafana、Prometheus、ELK Stack 也將延後實施。
架構上，YARP (API Gateway) 和微服務拆分會是未來階段的任務。
在功能面，語音合成服務如 Polly 和 QRCode 生成功能暫不實作。
此外，有影音、會員、好友制、檔案傳輸等功能已明確取消。
架構模式方面，DDD、乾淨架構、CQRS、JWT 以及 TDD 
也不會使用 AutoMapper 和 FluentValidation。
