import { Text } from '@chakra-ui/react'

interface MessageTimestampProps {
  timestamp: number
  isMe: boolean
}

const MessageTimestamp = ({ timestamp, isMe }: MessageTimestampProps) => {
  const timeStr = new Date(timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
  return (
    <Text
      variant='timestamp'
      ml={isMe ? 0 : 2}
      mr={isMe ? 2 : 0}
    >
      {timeStr}
    </Text>
  )
}

export default MessageTimestamp
