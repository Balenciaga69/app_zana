目前 我的 Frontend signalR service,hooks,store 那一段
被批評太難寫，需要重構
---
我希望晚點能重構一個V2版本且不要依賴v1的內容
---
我收到準則:
- 首先，不要讓 SignalR 的事件處理邏輯散佈在各個 Component 裡。這會導致程式碼難以維護且重用性低
- 建立 SignalRService, 這個服務將負責建立與 SignalR Hub 的連線、管理連線狀態，並監聽所有伺服器發送的事件。
- SignalR 服務收到事件後，不應該直接通知特定的 Component。相反地，它應該像一個事件發射器 (Event Emitter)。其他部分的程式碼可以訂閱 (subscribe) 它感興趣的事件。
- 所有的 SignalR 事件，無論是使用者更名、新訊息或打字狀態，都應該透過 SignalR 服務層，最終更新到 Zustand Store 裡。
- 將不同的狀態邏輯切分成不同的 "slice" 是 Zustand 的最佳實踐。例如，你可以建立