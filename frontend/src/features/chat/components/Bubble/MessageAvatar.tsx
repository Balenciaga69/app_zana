import { Avatar } from '@chakra-ui/react'

interface MessageAvatarProps {
  user: string
  isMe: boolean
}

const MessageAvatar = ({ user, isMe }: MessageAvatarProps) => (
  <Avatar
    size='sm'
    name={user}
    bg={isMe ? 'blue.400' : 'gray.400'}
    color='white'
    boxShadow='0 2px 8px 0 rgba(0,0,0,0.10)'
    ml={isMe ? 2 : 0}
    mr={!isMe ? 2 : 0}
    variant='message'
  />
)

export default MessageAvatar
