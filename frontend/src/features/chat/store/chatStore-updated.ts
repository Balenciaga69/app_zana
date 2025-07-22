// 更新後的 chatStore.ts - 移除對 chatService 的直接依賴
import type { ChatStore } from '@/features/chat/models/ChatStore'
import { create } from 'zustand'
import type { Message } from '../models/Message'

export const useChatStore = create<ChatStore>((set, get) => ({
  users: [],
  messages: [],
  currentUser: '',
  isConnected: false,
  connectionStatus: '未連線',

  setCurrentUser: (username: string) => {
    set({ currentUser: username })
  },

  sendMessage: async (text: string) => {
    // 這個方法現在會被 useChatService hook 覆寫
    console.warn('sendMessage 應該由 useChatService 提供')
  },

  addMessage: (message: Message) => {
    set((state) => ({
      messages: [...state.messages, message],
      users: state.users.includes(message.user) ? state.users : [...state.users, message.user],
    }))
  },

  setConnectionStatus: (status: string, connected: boolean) => {
    set({ connectionStatus: status, isConnected: connected })
  },
}))

// 移除 initializeSignalR，改由 useChatService hook 處理
