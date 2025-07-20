import { Flex, Box } from '@chakra-ui/react'
import MessageAvatar from './MessageAvatar'
import MessageTimestamp from './MessageTimestamp'
import MessageUserName from './MessageUserName'
import MessageBubble from './MessageBubble'

interface MessageRowProps {
  user: string
  text: string
  timestamp: number
  isMe: boolean
}

const MessageRowMe = ({ user, text, timestamp }: Omit<MessageRowProps, 'isMe'>) => (
  <Flex direction='row' justify='flex-end' align='flex-end' mb={3} px={2}>
    <MessageTimestamp timestamp={timestamp} isMe={true} />
    <Box maxW='75%' display='flex' flexDirection='column' alignItems='flex-end'>
      <MessageBubble text={text} isMe={true} />
    </Box>
    <MessageAvatar user={user} isMe={true} />
  </Flex>
)

const MessageRowOther = ({ user, text, timestamp }: Omit<MessageRowProps, 'isMe'>) => (
  <Flex direction='row' justify='flex-start' align='flex-end' mb={3} px={2}>
    <MessageAvatar user={user} isMe={false} />
    <Box maxW='75%' display='flex' flexDirection='column' alignItems='flex-start'>
      <MessageUserName user={user} />
      <MessageBubble text={text} isMe={false} />
    </Box>
    <MessageTimestamp timestamp={timestamp} isMe={false} />
  </Flex>
)

const MessageRow = (props: MessageRowProps) => {
  return props.isMe ? (
    <MessageRowMe user={props.user} text={props.text} timestamp={props.timestamp} />
  ) : (
    <MessageRowOther user={props.user} text={props.text} timestamp={props.timestamp} />
  )
}

export default MessageRow
