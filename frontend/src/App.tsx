import { ChakraProvider } from '@chakra-ui/react'
import { StrictMode, useEffect } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import ExampleChatRoomPage from './features/ChatRoom/pages/ExampleChatRoomPage'
import CreateRoomPage from './features/Room/pages/CreateRoomPage'
import HomePage from './features/Room/pages/HomePage'
import JoinRoomPage from './features/Room/pages/JoinRoomPage'
import theme from './Shared/theme.ts'

// FIXME: 舊版代碼需要更新

import { useSignalRConnection } from './features/SignalR/hooks/useSignalRConnection'
import { useRegisterUser } from './features/SignalR/hooks/useRegisterUser'

function App() {
  useSignalRConnection()
  const { registerUser } = useRegisterUser()

  useEffect(() => {
    registerUser()
  }, [registerUser])

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
