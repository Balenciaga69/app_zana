import { BrowserRouter, Routes, Route } from 'react-router-dom'
import ExampleChatRoomPage from './features/ChatRoom/pages/ExampleChatRoomPage'
import HomePage from './features/Room/pages/HomePage'
import CreateRoomPage from './features/Room/pages/CreateRoomPage'
import JoinRoomPage from './features/Room/pages/JoinRoomPage'
import SignalRStatus from './Shared/SignalR/SignalRStatus'

function App() {
  return (
    <BrowserRouter>
      <SignalRStatus />
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
