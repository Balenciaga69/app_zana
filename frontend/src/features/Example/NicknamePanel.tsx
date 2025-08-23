import { useUserStore } from '@/features/SignalR/store/userStore'
import { Alert, AlertIcon, Box, Button, HStack, Input, Text, VStack } from '@chakra-ui/react'
import { useEffect, useState } from 'react'
import { useOnNicknameUpdated, useSendUpdateNickname } from '../SignalR/hooks/useUpdateNickname'

export const NicknamePanel = () => {
  const nickname = useUserStore((s) => s.nickname)
  const [value, setValue] = useState(nickname ?? '')
  const { updateNickname, updating, error } = useSendUpdateNickname()

  useEffect(() => {
    setValue(nickname ?? '')
  }, [nickname])

  return (
    <Box w='100%' maxW='520px' p={4} borderWidth='1px' borderRadius='md'>
      <VStack align='stretch' spacing={3}>
        <HStack justify='space-between'>
          <Text fontWeight='bold'>目前暱稱</Text>
        </HStack>
        <Text data-testid='nickname-value'>{nickname ?? '(尚未設定)'}</Text>
        <HStack>
          <Input placeholder='輸入新暱稱' value={value} onChange={(e) => setValue(e.target.value)} maxLength={20} />
          <Button
            onClick={() => updateNickname(value)}
            isLoading={updating}
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
