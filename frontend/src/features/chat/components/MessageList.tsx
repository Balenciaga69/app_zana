// 訊息列表元件
import { VStack, Box, Text } from '@chakra-ui/react'
import { useChatStore } from '../store/chatStore'
import MessageRow from './Bubble/MessageRow'
import { useEffect, useRef } from 'react'

const MessageList = () => {
  const { messages, currentUser, isConnected } = useChatStore()
  const messagesEndRef = useRef<HTMLDivElement>(null)

  // 自動滾動到最新訊息
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' })
  }, [messages])

  return (
    <VStack align='stretch' spacing={2} h='100%' overflowY='auto' pb={2}>
      {!isConnected && (
        <Box textAlign='center' p={4}>
          <Text color='gray.500' fontSize='sm'>
            正在連線到聊天室...
          </Text>
        </Box>
      )}
      {messages.length === 0 && isConnected && (
        <Box textAlign='center' p={4}>
          <Text color='gray.500' fontSize='sm'>
            還沒有訊息，開始聊天吧！
          </Text>
        </Box>
      )}
      {messages.map((msg, idx) => {
        const isMe = msg.user === currentUser
        return <MessageRow key={idx} user={msg.user} text={msg.text} timestamp={msg.timestamp} isMe={isMe} />
      })}{' '}
      <div ref={messagesEndRef} />
    </VStack>
  )
}

export default MessageList
