import { useCallback, useEffect, useState } from 'react'
import axios from 'axios'
import { SignalREvents } from '../models/SignalREvents'
import SignalRService from '../services/signalrService'
import { useUserStore } from '../store/userStore'
import { extractErrorMessage } from '../utils/errorMessageHelper'
import { config } from '../../../Shared/config'

/**
 * 主動發送：更新暱稱
 */
export function useSendUpdateNickname() {
  const [updating, setUpdating] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const updateNickname = useCallback(async (nickname: string) => {
    setUpdating(true)
    setError(null)
    try {
      console.info('xZx config.api.baseUrl', config.api.baseUrl)
      await axios.put(`${config.api.baseUrl}/user/nickname`, { newNickname: nickname })
    } catch (err: unknown) {
      setError(extractErrorMessage(err, '暱稱更新失敗'))
    } finally {
      setUpdating(false)
    }
  }, [])

  return { updateNickname, updating, error }
}

/**
 * 被動監聽：SignalR NicknameUpdated 事件
 */
export function useOnNicknameUpdated() {
  const setNickname = useUserStore((state) => state.setNickname)
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
}
