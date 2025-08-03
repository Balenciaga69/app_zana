// Zustand 聊天室 store，整合 SignalR 即時通訊
import type { ChatStore } from '@/features/ChatRoom/models/ChatStore'
import { create } from 'zustand'
import type { Message } from '../models/Message'

export const useChatStore = create<ChatStore>((set, get) => ({
  users: [],
  messages: [],
  currentUser: '',
  isConnected: false,
  connectionStatus: '未連線',
  setCurrentUser: function (username: string): void {
    throw new Error('Function not implemented.')
  },
  sendMessage: function (text: string): void {
    throw new Error('Function not implemented.')
  },
  addMessage: function (message: Message): void {
    throw new Error('Function not implemented.')
  },
  setConnectionStatus: function (status: string, connected: boolean): void {
    throw new Error('Function not implemented.')
  },
}))
