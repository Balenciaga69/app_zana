import type { FC, ReactNode } from 'react'
import { AppContainer } from '../../../../Shared/components/layouts/AppContainer'

interface ChatWindowProps {
  children: ReactNode
}

/**
 * ChatWindow - 聊天室容器元件
 *
 * 現在基於 AppContainer 實作，保持向後相容性。
 * 直接使用 'chat' 變體來獲得原始的聊天室佈局。
 */
const ChatWindow: FC<ChatWindowProps> = ({ children }) => {
  return (
    <AppContainer variant='chat' testId='chat-window'>
      {children}
    </AppContainer>
  )
}

export default ChatWindow
