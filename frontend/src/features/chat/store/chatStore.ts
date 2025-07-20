// Zustand 聊天室 store，暫時用假資料，之後可串接 SignalR 或 WebSocket
import type { ChatStore } from '@/features/chat/models/ChatStore'
import { create } from 'zustand'
import type { Message } from '../models/Message'

export const useChatStore = create<ChatStore>((set, get) => ({
  users: ['Alice', 'Bob'],
  messages: [
    { user: 'Alice', text: '哈囉！', timestamp: Date.now() },
    { user: 'Bob', text: '你好！', timestamp: Date.now() },
  ],
  sendMessage: (text: string) => {
    const user = '你'
    const newMsg: Message = { user, text, timestamp: Date.now() }
    set((state) => ({ messages: [...state.messages, newMsg] }))
  },
}))
