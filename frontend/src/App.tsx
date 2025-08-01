import { BrowserRouter, Routes, Route } from 'react-router-dom'
import ExampleChatRoomPage from './features/ChatRoom/pages/ExampleChatRoomPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<ExampleChatRoomPage />} />
        {/* <Route path="/chat" element={<ChatRoomPage />} /> */}
        <Route path='/example' element={<ExampleChatRoomPage />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
