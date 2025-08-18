import { useCallback, useEffect, useState } from 'react'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'
import { useUserStore } from '../store/userStore'

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
      let msg = '暱稱更新失敗'
      // 嘗試從錯誤物件中提取訊息
      // @Copilot 把這一段邏輯做成獨立方法 開始 然後搬到Utils裡面
      if (
        err &&
        typeof err === 'object' &&
        'message' in err &&
        typeof (err as { message?: unknown }).message === 'string'
      ) {
        msg = (err as { message: string }).message ?? msg
      }
      // @Copilot 把這一段邏輯做成幫助方法 結束
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
