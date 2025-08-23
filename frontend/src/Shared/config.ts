export const config = {
  signalR: {
    hubUrl: import.meta.env.VITE_SIGNALR_HUB_URL,
  },
  api: {
    baseUrl: import.meta.env.VITE_API_BASE,
  },
}

export const isDevelopment = import.meta.env.DEV
