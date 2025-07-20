import { Avatar } from '@chakra-ui/react'
import { messageAvatarStyles } from '../../styles/componentStyles'

interface MessageAvatarProps {
  user: string
  isMe: boolean
}

const MessageAvatar = ({ user, isMe }: MessageAvatarProps) => (
  <Avatar
    name={user}
    size={'sm'}
    sx={messageAvatarStyles(isMe)}
  />
)

export default MessageAvatar
