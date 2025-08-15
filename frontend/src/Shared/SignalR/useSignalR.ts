import { useEffect, useCallback, useRef } from 'react'
import { signalRService } from './signalrService'
import { useSignalRStore, signalRSelectors } from './signalrStore'

export const useSignalR = () => {
  const {
    connectionState,
    connection,
    error,
    isConnecting,
    setConnectionState,
    setConnection,
    setError,
    setIsConnecting,
    reset,
  } = useSignalRStore()

  // 防止重複連線
  const hasConnectedRef = useRef(false)

  // 連線函數
  const connect = useCallback(async () => {
    if (hasConnectedRef.current || isConnecting || connectionState === 'connected') {
      return
    }
    hasConnectedRef.current = true
    setIsConnecting(true)
    setError(null)
    setConnectionState('connecting')
    try {
      const hubConnection = await signalRService.connect()
      setConnection(hubConnection)
      setConnectionState('connected')
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Connection failed'
      setError(errorMessage)
      setConnectionState('error')
      // eslint-disable-next-line no-console
      console.error('Failed to connect to SignalR:', err)
    } finally {
      setIsConnecting(false)
    }
  }, [isConnecting, connectionState, setConnectionState, setConnection, setError, setIsConnecting])

  // 斷線函數
  const disconnect = useCallback(async () => {
    try {
      await signalRService.disconnect()
      reset()
    } catch (err) {
      // eslint-disable-next-line no-console
      console.error('Failed to disconnect from SignalR:', err)
    }
  }, [reset])

  // 初始化連線
  useEffect(() => {
    const doConnect = async () => {
      const state = useSignalRStore.getState()
      if (hasConnectedRef.current || state.isConnecting || state.connectionState === 'connected') {
        return
      }
      hasConnectedRef.current = true
      state.setIsConnecting(true)
      state.setError(null)
      state.setConnectionState('connecting')
      try {
        const hubConnection = await signalRService.connect()
        state.setConnection(hubConnection)
        state.setConnectionState('connected')
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : 'Connection failed'
        state.setError(errorMessage)
        state.setConnectionState('error')
        // eslint-disable-next-line no-console
        console.error('Failed to connect to SignalR:', err)
      } finally {
        state.setIsConnecting(false)
      }
    }
    doConnect()

    // 清理函數
    return () => {
      signalRService.disconnect()
    }
  }, []) // 只在 mount/unmount 執行一次

  // 監控連線狀態變化
  useEffect(() => {
    const interval = setInterval(() => {
      const currentState = signalRService.getConnectionState()
      if (currentState !== connectionState) {
        setConnectionState(currentState)
      }
    }, 1000)

    return () => clearInterval(interval)
  }, [connectionState, setConnectionState])

  return {
    // 狀態
    connectionState,
    connection,
    error,
    isConnecting,

    // 計算屬性
    isConnected: signalRSelectors.isConnected(useSignalRStore.getState()),
    isDisconnected: signalRSelectors.isDisconnected(useSignalRStore.getState()),
    hasError: signalRSelectors.hasError(useSignalRStore.getState()),
    canSendMessage: signalRSelectors.canSendMessage(useSignalRStore.getState()),

    // 動作
    connect,
    disconnect,
  }
}

// TODO: 後續可以添加更多專用的 hooks
// export const useChatRoom = () => { ... }
// export const useRoomManagement = () => { ... }
