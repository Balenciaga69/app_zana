import { HStack, VStack } from '@chakra-ui/react'
import UserAvatar from '../atoms/ChatMessage/UserAvatar'
import MessageBubble from '../atoms/ChatMessage/MessageBubble'
import Timestamp from '../atoms/ChatMessage/Timestamp'
export interface MessageContentProps {
  message: string
  time: string
}
export interface ChatMessageProps extends MessageContentProps {
  username: string
  avatarUrl?: string
  isOwn?: boolean
}
export interface OwnMessageProps extends MessageContentProps {}
export interface OtherMessageProps extends MessageContentProps {}

export const ChatMessage = ({ username, avatarUrl, message, time, isOwn }: ChatMessageProps) => (
  <HStack data-testid='ChatMessage' justifyContent={isOwn ? 'flex-end' : 'flex-start'} spacing={2} w='100%'>
    {isOwn
      ? [
          <OwnMessage key='own-msg' message={message} time={time} />,
          <UserAvatar key='own-avatar' name={username} src={avatarUrl} />,
        ]
      : [
          <UserAvatar key='other-avatar' name={username} src={avatarUrl} />,
          <OtherMessage key='other-msg' message={message} time={time} />,
        ]}
  </HStack>
)

const OwnMessage = ({ message, time }: OwnMessageProps) => (
  <VStack data-testid='OwnMessage' align='flex-end' spacing={1} maxW='80%'>
    <MessageBubble isOwn>{message}</MessageBubble>
    <Timestamp time={time} />
  </VStack>
)

export const OtherMessage = ({ message, time }: OtherMessageProps) => (
  <VStack data-testid='ForeignMessage' align='flex-start' spacing={1} maxW='80%'>
    <MessageBubble>{message}</MessageBubble>
    <Timestamp time={time} />
  </VStack>
)

export default ChatMessage
