## SignalR 學習路線圖
這是一個為您量身打造的 SignalR 學習路線圖，旨在幫助您快速掌握其核心概念與實際應用，並配合一個簡單的實作課程。
### 1\. 核心概念與基礎
- **1.1. 什麼是 SignalR？**
        - 理解 SignalR 的定位：為 ASP.NET 開發者提供的 WebSocket 抽象層。
            - 為什麼需要 SignalR？解決即時通訊的痛點。
            - SignalR 的主要功能與應用場景（聊天室、即時通知、儀表板等）。
        - **1.2. SignalR 傳輸方式**
        - 理解 WebSocket、Server-Sent Events (SSE) 和 Long Polling。
            - SignalR 如何自動協商最佳傳輸方式。
            - 不同傳輸方式的優缺點。
        - **1.3. Hubs (集線器)**
        - Hub 的概念：伺服器端的核心通訊類別。
            - 如何定義 Hubs：繼承 `Hub` 類別。
            - Hubs 方法的呼叫：客戶端呼叫伺服器，伺服器呼叫客戶端。
        - **1.4. 連線管理**
        - `ConnectionId` 的作用與生命週期。
            - Hub Context (`IHubContext`)：從 Hub 外部發送訊息。
            - 分組 (Groups)：管理多個客戶端連線。
        ### 2\. 客戶端與伺服器端互動
- **2.1. 伺服器端設定**
        - 在 ASP.NET Core 專案中啟用 SignalR。
            - 配置 Hubs 路由。
            - 依賴注入與 Hub Context 的使用。
        - **2.2. JavaScript 客戶端**
        - 引入 SignalR JavaScript 客戶端函式庫。
            - 建立 Hub 連線：`new signalR.HubConnectionBuilder()`。
            - 啟動連線：`connection.start()`。
            - 註冊伺服器端方法：`connection.on()`。
            - 呼叫伺服器端方法：`connection.invoke()`。
        - **2.3. .NET 客戶端 (選修)**
        - 為非瀏覽器應用程式提供即時通訊能力。
            - 概念與 JavaScript 客戶端類似。
        ### 3\. 進階主題
- **3.1. 錯誤處理**
        - 連線錯誤、方法呼叫錯誤。
            - 客戶端與伺服器端的錯誤處理機制。
        - **3.2. 擴展性**
        - 多伺服器部署：背板 (Backplane) 的概念（Redis, Azure SignalR Service）。
            - 理解背板如何讓多個 SignalR 伺服器同步狀態和訊息。
        - **3.3. 身份驗證與授權 (未來 Jackson 階段過後考慮)**
        - 與 ASP.NET Core Identity 整合。
            - 在 Hubs 上應用 `[Authorize]` 屬性。
        - **3.4. 監控與日誌**
        - SignalR 的內建日誌。
            - 如何利用 Serilog 等工具整合 SignalR 日誌。