import React, { useState } from 'react'
import { VStack } from '@chakra-ui/react'
import RoomInput from '../components/atoms/RoomInput'
import RoomButton from '../components/atoms/RoomButton'
import { useNavigate } from 'react-router-dom'

const CreateRoomPage = () => {
  const navigate = useNavigate()
  const [roomName, setRoomName] = useState('')
  const [maxUsers, setMaxUsers] = useState('')
  const [password, setPassword] = useState('')

  const handleCreate = () => {
    // TODO: 送出建立房間資料
    navigate('/example')
  }

  return (
    <VStack spacing={4} minH='100vh' justify='center'>
      {/* 房間名稱 */}
      <RoomInput
        placeholder='房間名稱'
        value={roomName}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setRoomName(e.target.value)}
      />
      {/* 人數上限 */}
      <RoomInput
        placeholder='人數上限'
        value={maxUsers}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setMaxUsers(e.target.value)}
        type='number'
      />
      {/* 密碼 (可選) */}
      <RoomInput
        placeholder='密碼 (可選)'
        value={password}
        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
        type='password'
      />
      <RoomButton onClick={handleCreate}>建立</RoomButton>
    </VStack>
  )
}

export default CreateRoomPage
