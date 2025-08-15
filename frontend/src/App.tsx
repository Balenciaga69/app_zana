import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { useEffect } from 'react'
import { signalRService } from './Shared/SignalR/signalrService'
import ExampleChatRoomPage from './features/ChatRoom/pages/ExampleChatRoomPage'
import CreateRoomPage from './features/Room/pages/CreateRoomPage'
import HomePage from './features/Room/pages/HomePage'
import JoinRoomPage from './features/Room/pages/JoinRoomPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<HomePage />} />
        <Route path='/create' element={<CreateRoomPage />} />
        <Route path='/join' element={<JoinRoomPage />} />
        {/* <Route path="/chat" element={<ChatRoomPage />} /> */}
        <Route path='/example' element={<ExampleChatRoomPage />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
