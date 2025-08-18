import { useEffect } from 'react'
import SignalRService from '../services/SignalRService'
import { useSignalRStore } from '../store/signalrSlice'

/**
 * SignalR 連線與全域狀態監控 hook
 */
export function useSignalRConnection() {
  const setConnectionStatus = useSignalRStore((s) => s.setConnectionStatus)
  const setLastError = useSignalRStore((s) => s.setLastError)

  useEffect(() => {
    const service = SignalRService.getInstance()
    setConnectionStatus('connecting')
    service
      .connect()
      .then(() => setConnectionStatus('connected'))
      .catch((err) => {
        setConnectionStatus('error')
        setLastError(err?.message ?? 'SignalR connect error')
      })
    return () => {
      service.disconnect()
      setConnectionStatus('disconnected')
    }
  }, [setConnectionStatus, setLastError])
}
