// 訊息列表元件
import { VStack } from '@chakra-ui/react'
import { useChatStore } from '../store/chatStore'
import MessageRow from './Bubble/MessageRow'

const MessageList = () => {
  const messages = useChatStore((state) => state.messages)
  return (
    <VStack align='stretch' spacing={2} h='100%' overflowY='auto' pb={2}>
      {messages.map((msg, idx) => {
        const isMe = msg.user === '你' || msg.user === '我'
        return <MessageRow key={idx} user={msg.user} text={msg.text} timestamp={msg.timestamp} isMe={isMe} />
      })}
    </VStack>
  )
}

export default MessageList
