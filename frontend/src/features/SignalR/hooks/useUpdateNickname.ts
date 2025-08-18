import { useCallback } from 'react'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'

/**
 * 更新暱稱 hook
 */
export function useUpdateNickname() {
  // TODO: 可依需求注入 userStore
  const updateNickname = useCallback(async (nickname: string) => {
    const service = SignalRService.getInstance()
    await service.invoke(SignalREvents.NICKNAME_UPDATED, { nickname })
    // TODO: 可監聽暱稱更新事件，並更新 store
  }, [])
  return { updateNickname }
}
