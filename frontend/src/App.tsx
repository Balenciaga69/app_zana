import { Button, HStack } from '@chakra-ui/react'
import { useState } from 'react'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <HStack>
        <Button onClick={() => setCount(count + 1)}>Click me</Button>
        <Button>Click me \-\{count}</Button>
      </HStack>
    </>
  )
}

export default App
