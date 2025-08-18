// Zustand slice skeleton for SignalR
import { create, type StateCreator } from 'zustand'

export interface SignalRState {
  // properties
  connectionStatus: 'connected' | 'connecting' | 'disconnected' | 'reconnecting' | 'error'
  connectionId?: string
  reconnectAttempts: number
  lastError?: string
  // actions
  setConnectionStatus: (status: SignalRState['connectionStatus']) => void
  setConnectionId: (id?: string) => void
  setLastError: (error?: string) => void
  incrementReconnectAttempts: () => void
  resetReconnectAttempts: () => void
}

export const createSignalRSlice: StateCreator<SignalRState> = (set, get) => ({
  connectionStatus: 'disconnected',
  connectionId: undefined,
  reconnectAttempts: 0,
  lastError: undefined,

  setConnectionStatus: (status: SignalRState['connectionStatus']) => set({ connectionStatus: status }),

  setConnectionId: (id?: string) => set({ connectionId: id }),

  setLastError: (error?: string) => set({ lastError: error }),

  incrementReconnectAttempts: () => set((state) => ({ reconnectAttempts: state.reconnectAttempts + 1 })),

  resetReconnectAttempts: () => set({ reconnectAttempts: 0 }),
})

export const useSignalRStore = create<SignalRState>()(createSignalRSlice)
