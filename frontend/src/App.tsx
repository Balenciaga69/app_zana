import { ChakraProvider } from '@chakra-ui/react'
import { StrictMode, useEffect } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import ExampleChatRoomPage from './features/ChatRoom/pages/ExampleChatRoomPage'
import CreateRoomPage from './features/Room/pages/CreateRoomPage'
import HomePage from './features/Room/pages/HomePage'
import JoinRoomPage from './features/Room/pages/JoinRoomPage'
import { useRegisterUser } from './features/SignalR/hooks/useRegisterUser'
import { useSignalRConnection } from './features/SignalR/hooks/useSignalRConnection'
import theme from './Shared/theme.ts'
import { useSignalRStore } from './features/SignalR/store/SignalRStore.ts'
import { useOnNicknameUpdated } from './features/SignalR/hooks/useUpdateNickname.ts'

function App() {
  // 啟動觸發
  useSignalRConnection()
  useOnNicknameUpdated()
  const { registerUser } = useRegisterUser()
  const { connectionStatus } = useSignalRStore()
  useEffect(() => {
    if (connectionStatus === 'connected') {
      registerUser()
    }
  }, [registerUser, connectionStatus])

  return (
    <StrictMode>
      <ChakraProvider theme={theme}>
        <BrowserRouter>
          <Routes>
            <Route path='/' element={<HomePage />} />
            <Route path='/create' element={<CreateRoomPage />} />
            <Route path='/join' element={<JoinRoomPage />} />
            {/* <Route path="/chat" element={<ChatRoomPage />} /> */}
            <Route path='/example' element={<ExampleChatRoomPage />} />
          </Routes>
        </BrowserRouter>
      </ChakraProvider>
    </StrictMode>
  )
}

export default App
