---
applyTo: '**'
---
Coding standards, domain knowledge, and preferences that AI should follow.### 前端專案計畫書：匿名即時聊天室

#### 核心理念

- 無會員制：提供免登入、匿名、純文字的即時通訊體驗。
- 分房機制：使用者可自由進出房間，進行即時互動。
- 穩固與可擴展：具備可監控與持久化的能力，並為未來擴展預留彈性。

---

### 前端專屬

#### 必備技術棧

本專案以 React + TypeScript 為核心，採用 Vite 作為開發與打包工具。UI 採 Chakra UI，並以原子設計系統組件化。狀態管理使用 Zustand，API 請求統一由 Axios 處理，SignalR Client 負責即時通訊。路由採用 React Router Dom。程式碼品質由 ESLint、Prettier 維護。

#### 架構與設計原則

- 元件化：每個 React 元件獨立、單一職責。
- 原子設計：UI 組件自 Atom → Molecule → Organism。
- 狀態管理：Zustand 管理全域狀態，業務邏輯與 UI 分離。
- 樣式：以 Chakra UI 為主，局部樣式用 `sx` 或 `styled`。
- API 處理：所有 API 請求集中管理，錯誤處理與資料轉換與 UI 解耦。
- 程式碼品質：ESLint、Prettier，Hooks 謹慎使用（`useEffect`, `useCallback`, `useMemo`）。
- 路由：集中配置與管理。

#### 已取消或未來規劃（暫不實作）

初期不考慮影音、會員、好友制、檔案傳輸等功能。未來如有擴展需求再行評估。

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
