import { create } from 'zustand'
import type { HubConnection } from '@microsoft/signalr'
import { signalRService, type ConnectionState } from '../services/signalrService'

interface SignalRState {
  // 狀態
  connectionState: ConnectionState
  connection: HubConnection | null
  error: string | null
  isConnecting: boolean

  // 動作
  setConnectionState: (state: ConnectionState) => void
  setConnection: (connection: HubConnection | null) => void
  setError: (error: string | null) => void
  setIsConnecting: (isConnecting: boolean) => void
  reset: () => void
}

const initialState = {
  connectionState: 'disconnected' as ConnectionState,
  connection: null,
  error: null,
  isConnecting: false,
}
export const useSignalRStore = create<
  SignalRState & {
    connect: () => Promise<void>
    disconnect: () => Promise<void>
  }
>((set, get) => ({
  ...initialState,

  setConnectionState: (connectionState) => set({ connectionState }),
  setConnection: (connection) => set({ connection }),
  setError: (error) => set({ error }),
  setIsConnecting: (isConnecting) => set({ isConnecting }),
  reset: () => set(initialState),

  // 新增 connect action
  connect: async () => {
    const state = get()
    // 如果: 已經在連接中、已經連接、或已有連接，則不進行重複連接
    if (state.isConnecting || state.connectionState === 'connected' || state.connection) {
      return
    }
    // 設置連接狀態為 connecting
    set({ isConnecting: true, error: null, connectionState: 'connecting' })
    try {
      // 嘗試建立連接
      const hubConnection = await signalRService.connect()
      set({ connection: hubConnection, connectionState: 'connected' })
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Connection failed'
      set({ error: errorMessage, connectionState: 'error' })
    } finally {
      set({ isConnecting: false })
    }
  },
  // 新增 disconnect action
  disconnect: async () => {
    try {
      await signalRService.disconnect()
      set(initialState)
    } catch {
      set({ error: 'Disconnect failed' })
    }
  },
}))

// 方便的選擇器
export const signalRSelectors = {
  isConnected: (state: SignalRState) => state.connectionState === 'connected',
  isDisconnected: (state: SignalRState) => state.connectionState === 'disconnected',
  hasError: (state: SignalRState) => state.error !== null,
  canSendMessage: (state: SignalRState) => state.connectionState === 'connected' && state.connection !== null,
}
