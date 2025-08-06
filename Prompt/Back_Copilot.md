## 核心思想：
免登入,無會員,匿名,即時通訊,分房機制,純文字,可監控,可持久化,匿名聊天室,房間機制,即時通訊

## 已取消或不考慮的內容:
有影音,有會員,有好友制,可傳送檔案,AutoMapper,FluentValidation,DDD,乾淨架構,CQRS, JWT

## 未來再來考慮(很肯定會導入，但不是現在):
K8s,AWS,Polly,YARP(API Gateway),RabbitMQ,MassTransit,gRPC,微服務,Grafana,Prometheus,ELK Stack

## 必備技術：
C# .NET 8+,SignalR,EF Core,PostgreSQL,Redis,Serilog,MediatR,Docker,Health Checks,xUnit,Moq,FluentAssertions,Git,Swagger,GitHub Actions

## 目前階段需要具備的功能：
- 用戶可開房,進房,離開房間
- 用戶可發言,接收訊息
- 同一台電腦同一瀏覽器視為同一用戶
- 同一用戶可以同時存在多個房間
- 斷線可重連
- 歷史訊息查詢
- 分享房間連結
- 房主在開房時與房間內可設定配置(密碼、房間名稱、人數上限)
- 系統自動銷毀空房間
- 統計在線人數
- (未來)可能會生成QRCode

## 給後端工程師提醒：
- 已有健康檢查（PostgreSQL、RabbitMQ、Redis）
- 已有 EF Core DbContext、Entities 及各種 Entity 配置
- 已可執行 Migration 到 PostgreSQL
- 已有通用 API Response、Exception Filter、日誌紀錄
- 採用 Controller、MediatR（不使用 Service），並使用 Repository Pattern
- 不採用 DDD、乾淨架構、CQRS
- 希望你能 TDD，至少腦海中有測試想法再下手
- 目前為單體架構，未來會拆分成微服務 + 多DB
- 避免直接外鍵關聯: 不要用 navigation property, 只保留 ID 引用

### 關於 Feature 與 Shared
- 依據 Features 切分（而非 Controller、Service、Models...）
- 未來每個 Feature 會獨立成微服務
- 每個 Feature 資料夾包含：
  - Commands（含 Handler、Result，放同一檔案）
  - Queries（含 Handler、Result，放同一檔案）
  - Controller（僅連接 Command/Query，Req/Resp 也用 Command/Query）
  - Repositories（介面與實作放同一檔案）
- DbContext、Entities 統一放在 Infrastructure
- BuildingBlock 類通用元件未來可放在 Shared

## 參與者的開發環境與角色定位
- 開發環境：Windows + VSCode + Docker
- 角色：優秀的架構師級開發者，專注於架構層級實作
- 品質優先：工程品質 > 開發速度，避免一次性大量代碼