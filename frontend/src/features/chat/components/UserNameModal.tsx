// 使用者名稱設定組件
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  Button,
  Input,
  FormControl,
  FormLabel,
  useDisclosure,
  VStack,
  Text,
  useColorMode,
} from '@chakra-ui/react'
import { useState, useEffect } from 'react'
import { useChatStore } from '../store/chatStore'

const UserNameModal = () => {
  const { isOpen, onOpen, onClose } = useDisclosure()
  const [username, setUsername] = useState('')
  const [error, setError] = useState('')
  const { colorMode } = useColorMode()
  
  const { currentUser, setCurrentUser } = useChatStore()

  // 如果沒有設定使用者名稱，自動開啟彈窗
  useEffect(() => {
    if (!currentUser) {
      onOpen()
    }
  }, [currentUser, onOpen])

  const handleSave = () => {
    if (username.trim().length < 2) {
      setError('使用者名稱至少需要2個字元')
      return
    }
    
    setCurrentUser(username.trim())
    setError('')
    onClose()
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setUsername(e.target.value)
    if (error) setError('')
  }

  return (
    <Modal isOpen={isOpen} onClose={() => {}} closeOnOverlayClick={false}>
      <ModalOverlay />
      <ModalContent bg={colorMode === 'dark' ? 'gray.800' : 'white'}>
        <ModalHeader>設定使用者名稱</ModalHeader>
        <ModalBody>
          <VStack spacing={4}>
            <Text fontSize="sm" color="gray.600">
              請設定你在聊天室中要顯示的名稱
            </Text>
            <FormControl isInvalid={!!error}>
              <FormLabel>使用者名稱</FormLabel>
              <Input
                placeholder="輸入你的名稱..."
                value={username}
                onChange={handleChange}
                onKeyDown={(e) => e.key === 'Enter' && handleSave()}
                autoFocus
              />
              {error && (
                <Text color="red.500" fontSize="sm" mt={1}>
                  {error}
                </Text>
              )}
            </FormControl>
          </VStack>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="blue" onClick={handleSave}>
            確定
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  )
}

export default UserNameModal
