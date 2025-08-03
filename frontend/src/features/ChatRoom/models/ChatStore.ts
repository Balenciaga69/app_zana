import type { Message } from './Message'

export interface ChatStore {
  users: string[]
  messages: Message[]
  currentUser: string
  isConnected: boolean
  connectionStatus: string
  setCurrentUser: (username: string) => void
  sendMessage: (text: string) => void
  addMessage: (message: Message) => void
  setConnectionStatus: (status: string, connected: boolean) => void
}
