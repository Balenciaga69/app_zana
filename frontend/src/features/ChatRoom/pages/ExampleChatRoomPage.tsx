import { Box, Button, Flex, Text, useColorMode } from '@chakra-ui/react'
import { type FC, useState } from 'react'
import { ChatMessage } from '../components/molecules/ChatMessage'
import ChatWindow from '../components/organisms/ChatWindow'
import MessageInputBar from '../components/molecules/MessageInputBar'

const mockMessages = [
  {
    username: 'Bruce Wayne',
    avatarUrl: '',
    message: '大家好，這是我的第一則訊息！',
    time: '10:01',
    isOwn: false,
  },
  {
    username: 'Clark Kent',
    avatarUrl: '',
    message: 'Hi Bruce! 很高興見到你。',
    time: '10:02',
    isOwn: true,
  },
  {
    username: 'Diana Prince',
    avatarUrl: '',
    message: '大家早安～',
    time: '10:03',
    isOwn: false,
  },
]

const ExampleHeader: FC = () => {
  const { colorMode, toggleColorMode } = useColorMode()

  return (
    <Flex p='4' borderBottomWidth='1px' justifyContent='space-between' alignItems='center'>
      <Text fontWeight='bold'>聊天室標題</Text>
      <Button size='sm' onClick={toggleColorMode}>
        {colorMode}
      </Button>
    </Flex>
  )
}
const ExampleChatRoomPage = () => {
  const [messages, setMessages] = useState([...mockMessages, ...mockMessages, ...mockMessages])

  const handleSend = (message: string) => {
    setMessages([
      ...messages,
      {
        username: 'You',
        avatarUrl: '',
        message,
        time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        isOwn: true,
      },
    ])
  }

  return (
    <ChatWindow>
      {/* 這裡是未來放置 ChatHeader, ChatHistory, MessageInputBar 的地方 */}
      <ExampleHeader></ExampleHeader>
      <Flex direction='column' flex='1' p='4' overflowY='auto'>
        {messages.map((msg, idx) => (
          <ChatMessage key={idx} {...msg} />
        ))}
      </Flex>
      <Box p='4' borderTopWidth='1px'>
        <MessageInputBar onSend={handleSend} />
      </Box>
    </ChatWindow>
  )
}

export default ExampleChatRoomPage
