// 應用程式配置
export const config = {
  // SignalR 後端 URL - 請根據你的後端設置調整
  signalR: {
    hubUrl: 'http://localhost:5219/chat', // 或 https://localhost:7000/chathub
  }
}

// 開發環境檢測
export const isDevelopment = import.meta.env.DEV
