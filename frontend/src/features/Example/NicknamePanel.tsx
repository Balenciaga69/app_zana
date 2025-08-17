import { useUserStore } from '@/features/SignalR/store/userStore'
import { Alert, AlertIcon, Box, Button, HStack, Input, Spinner, Text, VStack } from '@chakra-ui/react'
import { useEffect, useState } from 'react'

export const NicknamePanel = () => {
  const { nickname, registering, updatingNickname, error } = useUserStore()
  const [value, setValue] = useState(nickname ?? '')
  const updateNickname = useUserStore((s) => s.updateNickname)

  useEffect(() => {
    setValue(nickname ?? '')
  }, [nickname])

  return (
    <Box w='100%' maxW='520px' p={4} borderWidth='1px' borderRadius='md'>
      <VStack align='stretch' spacing={3}>
        <HStack justify='space-between'>
          <Text fontWeight='bold'>目前暱稱</Text>
          {registering && (
            <HStack>
              <Spinner size='sm' />
              <Text fontSize='sm'>註冊中...</Text>
            </HStack>
          )}
        </HStack>
        <Text data-testid='nickname-value'>{nickname ?? '(尚未設定)'}</Text>
        <HStack>
          <Input placeholder='輸入新暱稱' value={value} onChange={(e) => setValue(e.target.value)} maxLength={20} />
          <Button
            onClick={() => updateNickname(value)}
            isLoading={updatingNickname}
            isDisabled={!value?.trim()}
            colorScheme='teal'
          >
            更新暱稱
          </Button>
        </HStack>
        {error && (
          <Alert status='error'>
            <AlertIcon />
            <Text fontSize='sm'>{error}</Text>
          </Alert>
        )}
      </VStack>
    </Box>
  )
}
