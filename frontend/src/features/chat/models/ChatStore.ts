import type { Message } from './Message'

export interface ChatStore {
  users: string[]
  messages: Message[]
  sendMessage: (text: string) => void
}
