import { ChakraProvider } from '@chakra-ui/react'
import { StrictMode, useEffect } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import ExampleChatRoomPage from './features/ChatRoom/pages/ExampleChatRoomPage'
import CreateRoomPage from './features/Room/pages/CreateRoomPage'
import HomePage from './features/Room/pages/HomePage'
import JoinRoomPage from './features/Room/pages/JoinRoomPage'
import { useSignalR } from './features/SignalR/hooks/useSignalR.ts'
import { useRegisterUser } from './features/SignalR/hooks/useRegisterUser'
import theme from './Shared/styles/theme.ts'

function App() {
  const { disconnect, connect } = useSignalR()
  const { registerUser } = useRegisterUser()

  useEffect(() => {
    const initializeSignalR = async () => {
      await connect()
      await registerUser()
    }
    initializeSignalR()
    return () => {
      disconnect()
    }
  }, [registerUser, disconnect, connect])

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
