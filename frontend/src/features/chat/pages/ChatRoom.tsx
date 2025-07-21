import { Box, Flex } from '@chakra-ui/react'
import { useEffect } from 'react'
import MessageList from '../components/MessageList.tsx'
import MessageInput from '../components/MessageInput.tsx'
import Header from '../components/Header.tsx'
import UserNameModal from '../components/UserNameModal.tsx'
import { useColorMode } from '@chakra-ui/react'
import { initializeSignalR } from '../store/chatStore'
import {
  chatRoomContainerStyles,
  chatRoomBoxStyles,
  chatRoomInnerBoxStyles,
  chatRoomMessageListBoxStyles,
  chatRoomInputBoxStyles,
} from '../styles/componentStyles'

const ChatRoom = () => {
  const { colorMode } = useColorMode()
  useEffect(() => {
    initializeSignalR()
    return () => {
      // 組件卸載時關閉 SignalR 連線
      import('../services/chatService').then(({ chatService }) => {
        chatService.stop()
      })
    }
  }, [])
  return (
    <>
      <Flex sx={chatRoomContainerStyles}>
        <Box sx={chatRoomBoxStyles(colorMode)}>
          <Header />
          <Box sx={chatRoomInnerBoxStyles}>
            <Box sx={chatRoomMessageListBoxStyles}>
              <MessageList />
            </Box>
            <Box sx={chatRoomInputBoxStyles}>
              <MessageInput />
            </Box>
          </Box>
        </Box>
      </Flex>
      <UserNameModal />
    </>
  )
}

export default ChatRoom
