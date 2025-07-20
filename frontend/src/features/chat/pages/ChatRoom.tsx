import { Box, Flex } from '@chakra-ui/react'
import MessageList from '../components/MessageList.tsx'
import MessageInput from '../components/MessageInput.tsx'
import Header from '../components/Header.tsx'
import UserNameModal from '../components/UserNameModal.tsx'
import { useColorMode } from '@chakra-ui/react'
import {
  chatRoomContainerStyles,
  chatRoomBoxStyles,
  chatRoomInnerBoxStyles,
  chatRoomMessageListBoxStyles,
  chatRoomInputBoxStyles,
} from '../styles/componentStyles'

const ChatRoom = () => {
  const { colorMode } = useColorMode()
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
