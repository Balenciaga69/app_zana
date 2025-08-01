import { Flex } from '@chakra-ui/react'
import type { FC, ReactNode } from 'react'
import { chatWindowStyles } from './ChatWindow.style'

interface ChatWindowProps {
  children: ReactNode
}

const ChatWindow: FC<ChatWindowProps> = ({ children }) => {
  return (
    <Flex sx={chatWindowStyles.container} data-testid='chat-window-container'>
      <Flex sx={chatWindowStyles.wrapper} data-testid='chat-window-wrapper'>
        {children}
      </Flex>
    </Flex>
  )
}

export default ChatWindow
