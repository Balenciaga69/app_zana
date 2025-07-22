// 更新 config.ts 加入 API 設定
// 應用程式配置
export const config = {
  signalR: {
    hubUrl: 'http://localhost:5219/chat',
  },
  api: {
    baseUrl: 'http://localhost:5219/api',
  },
}

// 開發環境檢測
export const isDevelopment = import.meta.env.DEV
