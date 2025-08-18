// Zustand slice skeleton for SignalR

import { create, type StateCreator } from 'zustand'
import { type SignalRConnectionStatus } from '../models/SignalRConnectionStatus'

export interface SignalRStore {
  // 狀態
  connectionStatus: SignalRConnectionStatus
  connectionId?: string
  reconnectAttempts: number
  lastError?: string
  // 動作
  setConnectionStatus: (status: SignalRConnectionStatus) => void
  setConnectionId: (id?: string) => void
  setLastError: (error?: string) => void
  incrementReconnectAttempts: () => void
  resetReconnectAttempts: () => void
}

export const createSignalRSlice: StateCreator<SignalRStore> = (set) => ({
  connectionStatus: 'disconnected',
  connectionId: undefined,
  reconnectAttempts: 0,
  lastError: undefined,

  setConnectionStatus: (status: SignalRConnectionStatus) => set({ connectionStatus: status }),

  setConnectionId: (id?: string) => set({ connectionId: id }),

  setLastError: (error?: string) => set({ lastError: error }),

  incrementReconnectAttempts: () => set((state) => ({ reconnectAttempts: state.reconnectAttempts + 1 })),

  resetReconnectAttempts: () => set({ reconnectAttempts: 0 }),
})

export const useSignalRStore = create<SignalRStore>()(createSignalRSlice)
