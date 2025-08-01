export const config = {
  signalR: {
    hubUrl: import.meta.env.VITE_SIGNALR_HUB_URL,
  },
}

export const isDevelopment = import.meta.env.DEV
