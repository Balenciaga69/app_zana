## 後端專案計畫書：匿名即時聊天室

### 核心理念

- 無會員制：免登入、匿名、純文字聊天。
- 即時通訊：支援即時訊息傳輸與分房機制。
- 可擴展性：可監控、可持久化，並具備未來微服務化的潛力。

---

### 後端專屬

#### 必備技術棧

這個專案將採用 C# .NET 8+ 作為核心框架，並利用 SignalR 實現即時通訊。資料庫部分，主資料庫選用 PostgreSQL 搭配 EF Core 進行物件關聯對應，同時以 Redis 處理快取與暫存資料。日誌紀錄會使用 Serilog，命令/查詢模式則由 MediatR 負責。專案全程使用 Docker 進行容器化，並內建 Health Checks 確保服務健康。品質把關方面，採用 xUnit、Moq、FluentAssertions 撰寫單元測試。版本控制採用 Git，並利用 GitHub Actions 實現自動化工作流程。Swagger 用於產生與維護 API 文件。

#### 架構模式

- 架構類型：單體架構（未來會轉為微服務）
- 設計模式：
  - 命令/查詢模式：使用 MediatR
  - 資料存取：Repository Pattern（介面與實作放同一檔案）
- 程式碼組織：
  - 依 Feature 切分，而非傳統分層（如 Controller, Service）
  - 每個 Feature 資料夾包含：`Commands`（含 Handler）、`Queries`（含 Handler）、`Controller`、`Repositories`
  - `DbContext` 與 `Entities` 集中於 `Infrastructure`
  - 通用元件放於 `Shared`
  - Controller 只負責連接 Command/Query，保持輕量化。
  - 用 CommunicationHub 作為單一 Real-time Hub
  - 跨微服務/Feature 通訊使用 RabbitMQ/MassTransit

#### 資料庫設計原則

- 避免直接外鍵關聯：只保留 ID 引用，不使用 Navigation Property。
- 通用欄位：所有 Entity 必須包含 `CreatedAt`、`UpdatedAt`、`Id`。

#### 日誌紀錄規範（Log Guideline）

- 一般業務、API、Command/Query 處理，統一交由 MediatR Pipeline 自動記錄，不需在 Handler/Controller/Service 額外寫 log。
- 例外與錯誤，統一由 Middleware 處理。
- RabbitMQ 消費、SignalR Hub 事件，僅於「業務關鍵」情境（如跨服務追蹤、訊息失敗）才可補充 log，並註明原因。
- 禁止在 Controller、Service、Handler 內重複記錄一般流程 log。
- 日誌格式、TraceId、結構化資料，皆統一用 IAppLogger。

#### 未來規劃（暫不實作）

專案初期暫不導入 K8s、AWS 部署，監控工具如 Grafana、Prometheus、ELK Stack 延後實施。架構上，YARP（API Gateway）與微服務拆分為未來任務。功能面如語音合成（Polly）、QRCode 生成暫不實作。影音、會員、好友制、檔案傳輸等功能已明確取消。DDD、乾淨架構、CQRS、JWT、TDD、AutoMapper、FluentValidation 皆不採用。

---

### 通用

#### 功能需求

- 房間管理：使用者可建立房間、加入房間、離開房間。房間可設定密碼、名稱、人數上限。系統自動銷毀無人的空房間。
- 使用者行為：使用者可發送與接收訊息，支援歷史訊息查詢。
- 連線與識別：支援斷線後重連，同瀏覽器視為同一用戶，同一用戶可同時進入多個房間。
- 資訊顯示：統計並顯示房間內的在線人數，提供房間連結分享功能。

#### 已完成基礎設施

- 健康檢查（PostgreSQL, Redis）
- EF Core DbContext、Entities 與各種配置
- 可執行 Migration 至 PostgreSQL
- 通用 API Response、Exception Filter、日誌紀錄功能
- Controller 僅連接 Command/Query，保持輕量
- CommunicationHub 作為單一 Real-time Hub
- 跨微服務/Feature 通訊使用 RabbitMQ/MassTransit

---

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
