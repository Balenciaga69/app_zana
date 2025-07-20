import { Text } from '@chakra-ui/react'

interface MessageUserNameProps {
  user: string
}

const MessageUserName = ({ user }: MessageUserNameProps) => <Text variant='userName'>{user}</Text>

export default MessageUserName
