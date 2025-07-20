# 聊天室 SignalR 整合說明

## 功能特性

✅ **已完成的功能：**
- SignalR 即時通訊整合
- 用戶名稱設定
- 連線狀態顯示
- 即時訊息收發
- 自動重連機制
- 訊息自動滾動
- 深色/淺色主題切換

## 使用方式

### 1. 啟動後端 (.NET SignalR Hub)
確保你的 .NET 後端運行在 `http://localhost:5000` 並且 SignalR Hub 路由為 `/chathub`

### 2. 啟動前端
```bash
pnpm dev
```

### 3. 設定用戶名稱
- 首次進入會自動彈出用戶名稱設定彈窗
- 輸入至少2個字元的用戶名稱

### 4. 開始聊天
- 在輸入框輸入訊息並按 Enter 或點擊發送按鈕
- 訊息會透過 SignalR 即時廣播給所有連線的用戶

## 配置說明

### 後端 URL 設定
如果你的後端運行在不同的 URL，請修改：
```typescript
// src/config/config.ts
export const config = {
  signalR: {
    hubUrl: 'http://localhost:5000/chathub', // 修改為你的後端 URL
  }
}
```

### 常見後端 URL：
- 開發環境 HTTP: `http://localhost:5000/chathub`
- 開發環境 HTTPS: `https://localhost:7000/chathub`

## 組件架構

```
ChatRoom (主頁面)
├── Header (標題與用戶資訊)
├── MessageList (訊息列表)
│   └── MessageRow (個別訊息)
├── MessageInput (輸入框)
└── UserNameModal (用戶名稱設定彈窗)
```

## SignalR 服務
- `ChatService`: 管理 SignalR 連線和訊息
- 自動重連機制
- 連線狀態監控
- 錯誤處理

## 狀態管理 (Zustand)
- `useChatStore`: 聊天室狀態管理
- 訊息存儲
- 用戶管理
- 連線狀態

## 測試建議

1. **多開瀏覽器視窗**測試即時通訊
2. **中斷後端**測試重連機制
3. **不同用戶名稱**測試訊息區分
4. **長訊息**測試 UI 響應式

## 故障排除

### 連線失敗
- 檢查後端是否運行
- 檢查 URL 設定是否正確
- 檢查 CORS 設定（後端需允許前端域名）

### 訊息無法發送
- 檢查用戶名稱是否已設定
- 檢查連線狀態是否為「已連線」
- 檢查瀏覽器開發者工具的錯誤訊息
