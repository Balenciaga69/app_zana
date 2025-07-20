// 聊天室主畫面，負責組合訊息列表、輸入區、使用者列表
import { Box, Flex } from '@chakra-ui/react'
import MessageList from '../components/MessageList.tsx'
import MessageInput from '../components/MessageInput.tsx'
import UserList from '../components/UserList.tsx'

const ChatRoom = () => {
  return (
    <Flex h='100vh' align='center' justify='center' bg='gray.50'>
      <Box w='400px' bg='white' boxShadow='md' borderRadius='md' p={4}>
        <UserList />
        <MessageList />
        <MessageInput />
      </Box>
    </Flex>
  )
}

export default ChatRoom
