// Zustand 聊天室 store，暫時用假資料，之後可串接 SignalR 或 WebSocket
import type { ChatStore } from '@/features/chat/models/ChatStore'
import { create } from 'zustand'
import type { Message } from '../models/Message'

const users =["秦始皇","北極熊"]

export const useChatStore = create<ChatStore>((set) => ({
  users: users,
  messages: [
    { user: users[0], text: `騎${users[1]}`, timestamp: Date.now() },
    { user: users[1], text: `騎${users[0]}`, timestamp: Date.now() },
  ],
  sendMessage: (text: string) => {
    const user = '你'
    const newMsg: Message = { user, text, timestamp: Date.now() }
    set((state) => ({ messages: [...state.messages, newMsg] }))
  },
}))
