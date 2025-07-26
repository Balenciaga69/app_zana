// Zustand 聊天室 store，整合 SignalR 即時通訊
import type { ChatStore } from '@/features/chat/models/ChatStore'
import { create } from 'zustand'
import type { Message } from '../models/Message'
import { chatService } from '../services/chatService'

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
    const { currentUser, isConnected } = get()

    if (!isConnected) {
      console.error('SignalR 未連線，無法發送訊息')
      return
    }

    if (!currentUser.trim()) {
      console.error('請先設定使用者名稱')
      return
    }

    try {
      // 透過 SignalR 發送訊息到後端
      await chatService.sendMessage(currentUser, text)
    } catch (error) {
      console.error('發送訊息失敗:', error)
    }
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

// 初始化 SignalR 連線和事件處理
export const initializeSignalR = async () => {
  const store = useChatStore.getState()

  // 設定訊息接收處理器
  chatService.setMessageHandler((user: string, message: string) => {
    const newMessage: Message = {
      user,
      text: message,
      timestamp: Date.now(),
    }
    store.addMessage(newMessage)
  })

  // 開始連線
  try {
    store.setConnectionStatus('連線中...', false)
    await chatService.start()
    store.setConnectionStatus('已連線', true)
  } catch (error) {
    console.error('SignalR 初始化失敗:', error)
    store.setConnectionStatus('連線失敗', false)
  }
}
