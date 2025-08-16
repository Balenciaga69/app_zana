import { Box, VStack } from '@chakra-ui/react'
import RoomButton from '../components/atoms/RoomButton'
import { useNavigate } from 'react-router-dom'
import HealthCheck from '@/features/HealthCheck/HealthCheck'
import SignalRStatus from '@/Shared/SignalR/components/SignalRStatus'
import { AppContainer } from '@/Shared/components/layouts/AppContainer'

const HomePage = () => {
  const navigate = useNavigate()
  return (
    <AppContainer variant='roomList' testId='home-page'>
      <HealthCheck />
      <SignalRStatus />
      <VStack spacing={4} justify='center' flex={1}>
        <RoomButton onClick={() => navigate('/create')}>建立房間</RoomButton>
        <RoomButton onClick={() => navigate('/join')}>加入房間</RoomButton>
      </VStack>
    </AppContainer>
  )
}

export default HomePage
