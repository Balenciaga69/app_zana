import { useEffect } from 'react'
import { SignalREvents } from '../services/SignalREvents'
import SignalRServiceV2 from '../services/SignalRServiceV2'
import { useUserStore } from '../store/userStore'

/**
 * 用戶相關 SignalR 事件 hook
 */
export function useUserSignalR() {
  const setUser = useUserStore(s => s.setUser)
  const setNickname = useUserStore(s => s.setNickname)

  useEffect(() => {
    const service = SignalRServiceV2.getInstance()
    // 註冊用戶註冊事件
    const onRegisterUser = (payload: any) => {
      setUser(payload)
    }
    // 註冊暱稱更新事件
    const onNicknameUpdated = (payload: any) => {
      setNickname(payload.nickname)
    }
    service.on(SignalREvents.REGISTER_USER, onRegisterUser)
    service.on(SignalREvents.NICKNAME_UPDATED, onNicknameUpdated)
    return () => {
      service.off(SignalREvents.REGISTER_USER, onRegisterUser)
      service.off(SignalREvents.NICKNAME_UPDATED, onNicknameUpdated)
    }
  }, [setUser, setNickname])
}
