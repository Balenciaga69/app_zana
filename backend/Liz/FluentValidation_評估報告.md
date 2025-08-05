## FluentValidation 導入評估

### 什麼是 FluentValidation？
FluentValidation 是一套 .NET 常用的驗證框架，支援以程式碼流暢式（Fluent API）撰寫複雜的驗證規則。它可取代傳統 DataAnnotations，並能與 MediatR Pipeline 行為整合，實現統一且可擴展的驗證機制。

---

### 專案現況
- 目前 Request Model（如 CreateUserRequest、FindUserByFingerprintRequest 等）已使用 DataAnnotations（如 `[Required]`, `[MaxLength]`）進行基本驗證。
- Controller 採用 `[ApiController]`，ASP.NET Core 會自動進行 ModelState 驗證。
- 驗證需求目前以欄位長度、必填為主，尚未出現複雜跨欄位或條件式驗證。

---

### FluentValidation 優缺點

**優點：**
- 支援複雜驗證邏輯（如跨欄位、條件式、客製訊息）。
- 驗證規則集中管理，與業務邏輯分離。
- 可與 MediatR Pipeline 整合，統一 Command/Query 驗證流程。
- 易於單元測試與重用。

**缺點：**
- 初期需為每個 Request Model 撰寫 Validator 類別，增加開發量。
- 若現有驗證僅為簡單 DataAnnotations，導入初期效益有限。
- 團隊需學習 FluentValidation API，驗證規則需搬移。

---

### 陣痛期與改動評估

- **初期導入**：需為每個 Request Model 新增對應 Validator 類別，現有 DataAnnotations 可保留或逐步移除。
- **Controller 端**：如與 MediatR Pipeline 整合，Controller 可移除 ModelState 檢查，驗證統一由 Pipeline 處理。
- **驗證規則搬移**：如僅用於簡單欄位驗證，搬移成本低；如有複雜驗證，FluentValidation 更具優勢。
- **整體改動幅度**：可採漸進式導入，先針對新功能或複雜驗證需求使用 FluentValidation，舊有功能維持 DataAnnotations。

---

### 適用時機

- 驗證需求日益複雜，DataAnnotations 難以維護時。
- 希望驗證規則集中管理、易於測試。
- 預計與 MediatR Pipeline 行為整合，統一驗證流程。
- 團隊有意提升驗證彈性與可維護性。

---

### 專案建議

- **短期**：現有 DataAnnotations 已能滿足基本需求，可暫不導入 FluentValidation。
- **中長期**：如驗證需求變複雜、需統一驗證流程，建議導入 FluentValidation 並與 MediatR Pipeline 整合。
- **導入策略**：可先針對新功能或複雜驗證場景導入 FluentValidation，逐步取代 DataAnnotations，降低一次性改動壓力。

---

### 套件安裝建議

| 套件名稱 | 是否建議 | 理由 |
|---|---|---|
| MediatR | 必裝 | MediatR 核心 |
| MediatR.Extensions.Microsoft.DependencyInjection | 必裝 | ASP.NET Core DI 整合 |
| FluentValidation | 選擇性 | 驗證需求複雜時再導入 |
| MediatR.Extensions.FluentValidation.AspNetCore | 選擇性 | 需用 FluentValidation 並與 MediatR 整合時才裝 |

---

### 結論

> **目前可維持 DataAnnotations，待驗證需求提升或團隊熟悉 MediatR 後，再漸進導入 FluentValidation。此策略可兼顧開發效率與未來擴展彈性，降低一次性改動風險。**
