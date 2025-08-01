import { Box, Flex } from '@chakra-ui/react'
import { useState } from 'react'
import { ChatMessage } from '../components/molecules/ChatMessage'
import ChatWindow from '../components/organisms/ChatWindow'
import MessageInputBar from '../components/molecules/MessageInputBar'
import ChatRoomHeader from '../components/organisms/ChatRoomHeader'
import DateSeparator from '../components/atoms/DateSeparator'

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
      <ChatRoomHeader />
      <Flex direction='column' flex='1' p='4' overflowY='auto'>
        {messages.map((msg, idx) => (
          <ChatMessage key={idx} {...msg} />
        ))}
        <DateSeparator date='2025-08-02' />
      </Flex>
      <Box p='4' borderTopWidth='1px'>
        <MessageInputBar onSend={handleSend} />
      </Box>
    </ChatWindow>
  )
}

export default ExampleChatRoomPage
