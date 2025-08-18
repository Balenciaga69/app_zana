import { useEffect } from 'react'
import { useSignalRStore } from '../store/signalrStore'
import SignalRService from '../services/signalrService'

/**
 * SignalR 連線與全域狀態監控 hook
 */
export function useSignalRConnection() {
  const setConnectionStatus = useSignalRStore((state) => state.setConnectionStatus)
  const setLastError = useSignalRStore((state) => state.setLastError)

  useEffect(() => {
    console.info('xZx useSignalRConnection start')
    const service = SignalRService.getInstance()
    setConnectionStatus('connected')
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
      console.info('xZx useSignalRConnection end')
    }
  }, [setConnectionStatus, setLastError])
}
