import { Box, VStack, HStack, Text, Button, Input, Alert, AlertIcon, Spinner } from '@chakra-ui/react'
import RoomButton from '../components/atoms/RoomButton'
import { useNavigate } from 'react-router-dom'
import HealthCheck from '@/features/HealthCheck/HealthCheck'
import SignalRStatus from '@/Shared/SignalR/components/SignalRStatus'
import { AppContainer } from '@/Shared/components/layouts/AppContainer'
import { useEffect, useState } from 'react'
import { useUserStore } from '@/features/User/store/userStore'

// 內嵌元件：暱稱顯示與更新
const NicknamePanel = () => {
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

const HomePage = () => {
  const navigate = useNavigate()

  return (
    <AppContainer variant='roomList' testId='home-page'>
      <HealthCheck />
      <SignalRStatus />
      <VStack spacing={4} justify='center' flex={1}>
        <NicknamePanel />
        <RoomButton onClick={() => navigate('/create')}>建立房間</RoomButton>
        <RoomButton onClick={() => navigate('/join')}>加入房間</RoomButton>
      </VStack>
    </AppContainer>
  )
}

export default HomePage
