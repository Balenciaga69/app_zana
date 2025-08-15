import { Box, Text, Spinner, Alert, AlertIcon } from '@chakra-ui/react'
import { useSignalR } from './useSignalR'

const SignalRStatus = () => {
  const { connectionState, error, isConnected } = useSignalR()

  if (connectionState === 'connecting') {
    return (
      <Box position='fixed' top={4} right={4} zIndex={9999}>
        <Alert status='info' borderRadius='md' maxW='200px'>
          <Spinner size='sm' mr={2} />
          <Text fontSize='sm'>連線中...</Text>
        </Alert>
      </Box>
    )
  }

  if (error) {
    return (
      <Box position='fixed' top={4} right={4} zIndex={9999}>
        <Alert status='error' borderRadius='md' maxW='250px'>
          <AlertIcon />
          <Text fontSize='sm'>連線失敗: {error}</Text>
        </Alert>
      </Box>
    )
  }

  if (isConnected) {
    return (
      <Box position='fixed' top={4} right={4} zIndex={9999}>
        <Alert status='success' borderRadius='md' maxW='200px'>
          <AlertIcon />
          <Text fontSize='sm'>SignalR 已連線</Text>
        </Alert>
      </Box>
    )
  }

  return null
}

export default SignalRStatus
