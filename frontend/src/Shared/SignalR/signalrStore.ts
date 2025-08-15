import { create } from 'zustand'
import type { HubConnection } from '@microsoft/signalr'
import type { ConnectionState } from './signalrService'

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

export const useSignalRStore = create<SignalRState>((set) => ({
  ...initialState,

  setConnectionState: (connectionState) => set({ connectionState }),
  setConnection: (connection) => set({ connection }),
  setError: (error) => set({ error }),
  setIsConnecting: (isConnecting) => set({ isConnecting }),
  reset: () => set(initialState),
}))

// 方便的選擇器
export const signalRSelectors = {
  isConnected: (state: SignalRState) => state.connectionState === 'connected',
  isDisconnected: (state: SignalRState) => state.connectionState === 'disconnected',
  hasError: (state: SignalRState) => state.error !== null,
  canSendMessage: (state: SignalRState) => state.connectionState === 'connected' && state.connection !== null,
}
