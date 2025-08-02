import { Input } from '@chakra-ui/react'
import React from 'react'
import { messageInputStyles } from './MessageInput.style'

interface MessageInputProps {
  value: string
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void
  placeholder?: string
}

const MessageInput: React.FC<MessageInputProps> = ({ value, onChange, placeholder }) => {
  return (
    <Input
      type='text'
      value={value}
      onChange={onChange}
      placeholder={placeholder ?? 'Type a message...'}
      sx={messageInputStyles.input}
      data-testid='MessageInput'
    />
  )
}

export default MessageInput
