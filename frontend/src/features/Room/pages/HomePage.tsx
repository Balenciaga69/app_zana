import { AppContainer } from '@/features/AppContainer'
import HealthCheck from '@/features/Example/HealthCheck'
import { NicknamePanel } from '@/features/Example/NicknamePanel'
import SignalRStatus from '@/features/Example/SignalRStatus'
import { VStack } from '@chakra-ui/react'
import { useNavigate } from 'react-router-dom'
import RoomButton from '../components/atoms/RoomButton'

const HomePage = () => {
  const navigate = useNavigate()

  return (
    <AppContainer testId='home-page'>
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
