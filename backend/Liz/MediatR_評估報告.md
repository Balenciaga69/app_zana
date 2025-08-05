# MediatR 導入評估報告

## 什麼是 MediatR？
MediatR 是 .NET 生態系常用的中介者模式（Mediator Pattern）實現，主要用於解耦 Controller、Service、Domain 之間的直接依賴，讓請求（Command/Query/Event）與處理邏輯分離，提升系統可維護性與擴展性。

---

## 專案現況摘要
- 採用 .NET 8、Feature Folder 結構，單體架構，已規劃 Service/Controller 分層。
- 目標為高可維護、可擴展、低耦合，工程品質優先。
- 專案規劃文件（如 `Common_Plan.md`、`Backend_Copilot.md`）多次提及 MediatR 為推薦/建議導入套件。
- 目前尚未看到 MediatR 相關實作，Service 直接注入於 Controller。

---

## MediatR 優點
- **解耦**：Controller 不需直接依賴 Service，改以 Command/Query/Notification 傳遞，降低耦合。
- **單一職責**：每個 Handler 負責單一請求，易於維護與測試。
- **橫切面擴展**：可集中處理 Logging、Validation、Transaction、Cache 等（Pipeline Behavior）。
- **易於測試**：Handler 可獨立單元測試，Mock MediatR 介面即可。
- **支援 CQRS**：自然支援 Command/Query 分離，利於大型/複雜業務。
- **未來可平滑轉型微服務**：事件/通知模式易於與 MassTransit、RabbitMQ 等整合。

## MediatR 缺點
- **學習曲線**：需理解中介者模式、CQRS 概念，對新手較陌生。
- **初期開發速度略慢**：需額外建立 Request/Handler 類別，CRUD 小功能顯得繁瑣。
- **過度抽象風險**：小型/簡單專案若過度拆分，反而增加維護成本。
- **Debug Trace 較間接**：調用鏈不再是 Controller→Service，需追蹤 MediatR 流程。

---

## 適用時機
- **專案需高可維護性、可擴展性**
- **預期未來功能會快速增長、需求變動大**
- **有多種橫切面需求（如驗證、日誌、權限）**
- **團隊開發、多人協作**
- **有意導入 CQRS 或事件驅動架構**

---

## 專案建議
### 綜合評估：**建議導入 MediatR**
- 你的專案明確追求低耦合、可維護、可擴展，且規劃文件已預留 MediatR 擴展點。
- 雖然目前為單體 MVP，但未來有拆分/擴展需求，MediatR 可作為穩定基礎。
- 建議初期僅針對複雜業務（如房間、聊天、用戶）導入，簡單 CRUD 可先維持現狀，待團隊熟悉後再全面推廣。
- 可搭配 Pipeline Behavior 實現統一驗證、日誌、權限等橫切面。

---

## 實作建議
- 先導入 MediatR 套件，建立基本 Command/Query/Handler 範本。
- Controller 只負責接收請求，轉交 MediatR，Service/Repository 由 Handler 注入。
- 逐步將複雜業務遷移至 Handler，保留現有 Service 以利平滑過渡。
- 撰寫單元測試時，直接測 Handler 邏輯。

---

## 參考
- [MediatR 官方文件](https://github.com/jbogard/MediatR)
- [CQRS 與中介者模式介紹](https://docs.microsoft.com/zh-tw/azure/architecture/patterns/mediator)

---

> **結論：本專案高度建議導入 MediatR，並以漸進式方式推動，兼顧工程品質與開發效率。**


## 導入 MediatR 步驟
- 於專案根目錄執行 NuGet 安裝 MediatR 及 MediatR.Extensions.Microsoft.- DependencyInjection 套件。
- 在 Startup/Program.cs 內註冊 MediatR，指定 Handler 掃描的組件。
- 於 Feature Folder 下建立 Requests（Command/Query）與 Handlers 資料夾。
- 為每個複雜業務建立 Command/Query 類別（繼承 IRequest<T>）。
- 為每個 Command/Query 建立對應 Handler（實作 IRequestHandler<TRequest, TResponse>）。
- Controller 只注入 IMediator，將請求包裝成 Command/Query，呼叫 mediator.Send()。
- Handler 內注入 Service/Repository，實作實際業務邏輯。
- 若需橫切面（如驗證、日誌），可實作 Pipeline Behavior。
- 撰寫單元測試，直接測 Handler。
- 逐步將現有 Service 呼叫遷移至 MediatR，保留原有 Service 以利平滑過渡。
