import { Box, Flex } from '@chakra-ui/react'
import MessageList from '../components/MessageList.tsx'
import MessageInput from '../components/MessageInput.tsx'
import Header from '../components/Header.tsx'
import {
  chatRoomContainerStyles,
  chatRoomBoxStyles,
  chatRoomInnerBoxStyles,
  chatRoomMessageListBoxStyles,
  chatRoomInputBoxStyles,
} from '../styles/componentStyles'

const ChatRoom = () => {
  return (
    <Flex sx={chatRoomContainerStyles}>
      <Box sx={chatRoomBoxStyles}>
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
  )
}

export default ChatRoom
