import React, { useState } from 'react'
import { VStack } from '@chakra-ui/react'
import RoomInput from '../components/atoms/RoomInput'
import RoomButton from '../components/atoms/RoomButton'
import { useNavigate } from 'react-router-dom'

const JoinRoomPage = () => {
  const navigate = useNavigate()
  const [roomCode, setRoomCode] = useState('')
  const [password, setPassword] = useState('')

  const handleJoin = () => {
    // TODO: 送出加入房間資料
    navigate('/example')
  }

  return (
    <VStack spacing={4} minH='100vh' justify='center'>
      {/* 房間網址/代碼 */}
      <RoomInput
        placeholder='房間網址/代碼'
        value={roomCode}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setRoomCode(e.target.value)}
      />
      {/* 密碼 (如有) */}
      <RoomInput
        placeholder='密碼 (如有)'
        value={password}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
        type='password'
      />
      <RoomButton onClick={handleJoin}>加入</RoomButton>
    </VStack>
  )
}

export default JoinRoomPage
