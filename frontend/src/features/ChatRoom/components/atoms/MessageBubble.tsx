import { Box, type BoxProps } from '@chakra-ui/react'
import type { ReactNode } from 'react'
import { messageBubbleStyles } from './MessageBubble.style'

export interface MessageBubbleProps extends BoxProps {
  isOwn?: boolean
  children: ReactNode
}

export const MessageBubble = ({ isOwn, children, ...rest }: MessageBubbleProps) => {
  const style = isOwn ? messageBubbleStyles.own : messageBubbleStyles.other
  return (
    <Box data-testid='MessageBubble' sx={style} {...rest}>
      {children}
    </Box>
  )
}

export default MessageBubble
