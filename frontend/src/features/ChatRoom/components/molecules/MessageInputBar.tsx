import { Box } from '@chakra-ui/react'
import { messageInputBarStyles } from './MessageInputBar.style'
import React, { useState } from 'react'
import MessageInput from '../atoms/MessageInputBar/MessageInput'
import SendButton from '../atoms/MessageInputBar/SendButton'

interface MessageInputBarProps {
  onSend: (message: string) => void
}

const MessageInputBar: React.FC<MessageInputBarProps> = ({ onSend }) => {
  const [input, setInput] = useState('')

  const handleSend = () => {
    if (input.trim()) {
      onSend(input)
      setInput('')
    }
  }

  return (
    <Box sx={messageInputBarStyles.container}>
      <MessageInput value={input} onChange={(e) => setInput(e.target.value)} placeholder='Type a message...' />
      <SendButton onClick={handleSend} disabled={!input.trim()}>
        Send
      </SendButton>
    </Box>
  )
}

export default MessageInputBar
