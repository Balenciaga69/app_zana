// 更新後的 ChatRoom.tsx
import { Box, Flex } from '@chakra-ui/react'
import MessageList from '../components/MessageList.tsx'
import MessageInput from '../components/MessageInput.tsx'
import Header from '../components/Header.tsx'
import UserNameModal from '../components/UserNameModal.tsx'
import { useColorMode } from '@chakra-ui/react'
import { useChatService } from '../hooks/useChatService'
import {
  chatRoomContainerStyles,
  chatRoomBoxStyles,
  chatRoomInnerBoxStyles,
  chatRoomMessageListBoxStyles,
  chatRoomInputBoxStyles,
} from '../../../styles/componentStyles.ts'

const ChatRoom = () => {
  const { colorMode } = useColorMode()
  // useChatService 會自動處理連線的建立與清理
  const chatService = useChatService()

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
              <MessageInput chatService={chatService} />
            </Box>
          </Box>
        </Box>
      </Flex>
      <UserNameModal />
    </>
  )
}

export default ChatRoom
