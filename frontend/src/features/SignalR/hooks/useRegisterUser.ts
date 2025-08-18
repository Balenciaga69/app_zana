import { useCallback } from 'react'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'

/**
 * 註冊用戶 hook
 */
export function useRegisterUser() {
  // TODO: 可依需求注入 userStore
  const registerUser = useCallback(async (payload?: any) => {
    const service = SignalRService.getInstance()
    await service.invoke(SignalREvents.REGISTER_USER, payload)
    // TODO: 可監聽註冊回傳事件，並更新 store
  }, [])
  return { registerUser }
}
