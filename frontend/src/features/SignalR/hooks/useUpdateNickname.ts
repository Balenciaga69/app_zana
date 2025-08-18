import { useCallback, useEffect } from 'react'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'
import { useUserStore } from '../store/userStore'

/**
 * 更新暱稱 hook
 */
export function useUpdateNickname() {
  const setNickname = useUserStore((state) => state.setNickname)

  // 包裝 invoke
  const updateNickname = useCallback(async (nickname: string) => {
    const service = SignalRService.getInstance()
    await service.invoke(SignalREvents.NICKNAME_UPDATED, { nickname })
  }, [])

  useEffect(() => {
    const service = SignalRService.getInstance()
    // 註冊 NicknameUpdated 事件
    const onNicknameUpdated = (newNickname: string) => {
      setNickname(newNickname)
    }
    service.on(SignalREvents.NICKNAME_UPDATED, onNicknameUpdated)
    return () => {
      service.off(SignalREvents.NICKNAME_UPDATED, onNicknameUpdated)
    }
  }, [setNickname])

  return { updateNickname }
}
