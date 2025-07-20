// 應用程式配置
export const config = {
  signalR: {
    hubUrl: 'http://localhost:5219/chat',
  },
}

// 開發環境檢測
export const isDevelopment = import.meta.env.DEV
