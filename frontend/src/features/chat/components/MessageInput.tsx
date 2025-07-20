// 訊息輸入元件
import { HStack, Input, IconButton, useColorMode, useToast } from '@chakra-ui/react'
import { useState } from 'react'
import { useChatStore } from '../store/chatStore'
import { FiSend, FiCamera } from 'react-icons/fi'
import { messageInputContainerStyles, messageInputStyles } from '../styles/componentStyles'

const MessageInput = () => {
  const [text, setText] = useState('')
  const { sendMessage, isConnected, currentUser } = useChatStore()
  const { colorMode } = useColorMode()
  const toast = useToast()

  const handleSend = async () => {
    if (!text.trim()) return

    if (!isConnected) {
      toast({
        title: '連線錯誤',
        description: 'SignalR 未連線，無法發送訊息',
        status: 'error',
        duration: 3000,
        isClosable: true,
      })
      return
    }

    try {
      await sendMessage(text)
      setText('')
    } catch (error) {
      toast({
        title: '發送失敗',
        description: '訊息發送失敗，請重試',
        status: 'error',
        duration: 3000,
        isClosable: true,
      })
    }
  }

  return (
    <HStack sx={messageInputContainerStyles(colorMode)}>
      <IconButton aria-label='拍照' icon={<FiCamera />} variant='messageAction' colorScheme='gray' />
      <Input
        placeholder='輸入訊息...'
        value={text}
        onChange={(e) => setText(e.target.value)}
        onKeyDown={(e) => e.key === 'Enter' && handleSend()}
        sx={messageInputStyles(colorMode)}
        flex={1}
        isDisabled={!isConnected || !currentUser}
      />
      <IconButton
        aria-label='發送'
        icon={<FiSend />}
        variant='messageAction'
        onClick={handleSend}
        isDisabled={!isConnected || !currentUser || !text.trim()}
      />
    </HStack>
  )
}

export default MessageInput
