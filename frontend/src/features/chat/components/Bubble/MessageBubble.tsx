import { Box, Text, useColorMode } from '@chakra-ui/react'
import { messageBubbleStyles } from '../../../../styles/componentStyles'

interface MessageBubbleProps {
  text: string
  isMe: boolean
}

const MessageBubble = ({ text, isMe }: MessageBubbleProps) => {
  const { colorMode } = useColorMode()
  return (
    <Box sx={messageBubbleStyles(colorMode, isMe)}>
      <Text fontSize='md' wordBreak='break-word'>
        {text}
      </Text>
    </Box>
  )
}

export default MessageBubble
