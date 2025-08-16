import { VStack } from '@chakra-ui/react'
import RoomButton from '../components/atoms/RoomButton'
import { useNavigate } from 'react-router-dom'
import SignalRStatus from '@/features/SignalR/components/SignalRStatus'
import HealthCheck from '@/features/HealthCheck/HealthCheck'

const HomePage = () => {
  const navigate = useNavigate()
  return (
    <>
      <HealthCheck />
      <SignalRStatus />
      <VStack spacing={4} minH='100vh' justify='center'>
        <RoomButton onClick={() => navigate('/create')}>建立房間</RoomButton>
        <RoomButton onClick={() => navigate('/join')}>加入房間</RoomButton>
      </VStack>
    </>
  )
}

export default HomePage
