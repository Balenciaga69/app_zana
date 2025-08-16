import { useState, useCallback } from 'react'
import { DeviceFingerprintHelper } from '@/features/SignalR/utils/deviceFingerprintHelper'
import { signalRService } from '@/features/SignalR/services/signalrService'

/**
 * 通用 SignalR 使用者註冊 hook
 * 提供 registerUser 方法與 loading/error 狀態
 */
export function useRegisterUser() {
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<null | Error>(null)

  const registerUser = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const fingerprint = await DeviceFingerprintHelper.getFingerprint()
      console.info('xZx fingerprint', fingerprint)
      await signalRService.registerUser(fingerprint)
    } catch (err) {
      setError(err instanceof Error ? err : new Error(String(err)))
    } finally {
      setLoading(false)
    }
  }, [])

  return { registerUser, loading, error }
}
