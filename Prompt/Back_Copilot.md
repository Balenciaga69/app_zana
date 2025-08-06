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
- 斷線可重連
- 歷史訊息查詢
- 分享房間連結
- 開房時可設定配置
- 系統自動銷毀空房間
- 統計在線人數
- (未來)可能會生成QRCode

## 給後端工程師提醒：
- 已有健康檢查(postgresql,rabbitmq,redis)
- 已有 EF Core DbContext,Entities,各種 Entity 的配置
- 已可執行 Migration 到 PostgreSQL
- 已有通用 API Response
- 已有通用 Exception Filter
- 已有通用日誌紀錄
- 採用 Controller,MediatR(不使用 Service),會用Repository Pattern
- DDD 跟 乾淨架構 還有 CQRS 都很棒，但我們不採用
- 我希望你能 TDD，至少腦海中有測試甚麼的想法再來下手
- 目前是單體架構，但遲早會拆分成微服務 + 多DB
- 按照 Features 分類管理

## 參與者的開發環境與角色定位
- 開發環境：Windows + VSCode + Docker
- 角色：優秀的架構師級開發者，專注於架構層級實作
- 品質優先：工程品質 > 開發速度，避免一次性大量代碼