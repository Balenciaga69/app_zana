import { useCallback } from 'react'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'
import { DeviceFingerprintHelper } from '../utils/deviceFingerprintHelper'
import { useSignalRStore } from '../store/SignalRStore'

/**
 * 註冊用戶 hook
 */
export function useRegisterUser() {
  const setConnectionStatus = useSignalRStore((state) => state.setConnectionStatus)
  const registerUser = useCallback(async () => {
    const service = SignalRService.getInstance()
    try {
      const fingerprint = await DeviceFingerprintHelper.getFingerprint()
      await service.invoke(SignalREvents.REGISTER_USER, fingerprint)
    } catch {
      // 失敗就斷線
      service.disconnect()
      setConnectionStatus('disconnected')
    }
  }, [setConnectionStatus])

  return { registerUser }
}
