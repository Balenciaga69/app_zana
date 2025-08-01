import { Avatar, type AvatarProps } from '@chakra-ui/react'
import { type FC } from 'react'

interface UserAvatarProps extends AvatarProps {
  name: string
}

const UserAvatar: FC<UserAvatarProps> = ({ name, ...rest }) => {
  return <Avatar name={name} size='sm' data-testid='UserAvatar' data-user-name={name} {...rest} />
}

export default UserAvatar
