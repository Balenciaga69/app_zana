import { Box, Button, Flex } from '@chakra-ui/react'
import { useState } from 'react'

const API_BASE = import.meta.env.VITE_API_BASE

export default function HealthCheck() {
  const [status, setStatus] = useState<string>('尚未檢查')
  const [loading, setLoading] = useState(false)

  const checkHealth = async () => {
    setLoading(true)
    setStatus('檢查中...')
    try {
      const res = await fetch(`${API_BASE}/health`)
      if (res.ok) {
        setStatus('後端健康 (Healthy)')
      } else {
        setStatus('後端異常 (Unhealthy)')
      }
    } catch {
      setStatus('無法連線到後端')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Flex p={4} align={'center'} justify='start' gap={4}>
      <Button onClick={checkHealth} disabled={loading}>
        {loading ? '檢查中...' : '健康檢查'}
      </Button>
      <Box>狀態：{status}</Box>
    </Flex>
  )
}
