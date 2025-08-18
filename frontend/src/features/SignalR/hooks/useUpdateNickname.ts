import { useCallback, useEffect, useState } from 'react'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'
import { useUserStore } from '../store/userStore'
import { extractErrorMessage } from '../utils/errorMessageHelper'

/**
 * 更新暱稱 hook
 */
export function useUpdateNickname() {
  const setNickname = useUserStore((state) => state.setNickname)
  const [updating, setUpdating] = useState(false)
  const [error, setError] = useState<string | null>(null)

  // 包裝 invoke 並處理 loading/error 狀態
  const updateNickname = useCallback(async (nickname: string) => {
    setUpdating(true)
    setError(null)
    const service = SignalRService.getInstance()
    try {
      await service.invoke(SignalREvents.UPDATE_NICKNAME, nickname)
    } catch (err: unknown) {
      // 預設錯誤訊息
      const msg = extractErrorMessage(err, '暱稱更新失敗')
      setError(msg)
    } finally {
      setUpdating(false)
    }
  }, [])

  // NicknameUpdated
  useEffect(() => {
    const service = SignalRService.getInstance()
    const onNicknameUpdated = (newNickname: string) => {
      setNickname(newNickname)
    }
    service.on(SignalREvents.NICKNAME_UPDATED, onNicknameUpdated)
    return () => {
      service.off(SignalREvents.NICKNAME_UPDATED, onNicknameUpdated)
    }
  }, [setNickname])

  return { updateNickname, updating, error }
}
