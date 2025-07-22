// React Hook 模式的 ChatService
import { useEffect, useRef, useCallback } from 'react'
import * as signalR from '@microsoft/signalr'
import { config } from '@/config/config'
import { useChatStore } from '../store/chatStore'
import type { Message } from '../models/Message'

const SIGNALR_EVENTS = {
  RECEIVE_MESSAGE: 'ReceiveMessage',
  SEND_MESSAGE: 'SendMessage',
  GET_CONNECTION_ID: 'GetConnectionId',
} as const

export const useChatService = () => {
  const connectionRef = useRef<signalR.HubConnection | null>(null)
  const retryCountRef = useRef(0)
  const { addMessage, setConnectionStatus } = useChatStore()

  const createConnection = useCallback(() => {
    if (connectionRef.current) return connectionRef.current

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      .withAutomaticReconnect()
      .build()

    // 設定事件處理器
    connection.on(SIGNALR_EVENTS.RECEIVE_MESSAGE, (user: string, message: string) => {
      console.log('[ChatService] 收到訊息:', user, message)
      const newMessage: Message = {
        user,
        text: message,
        timestamp: Date.now(),
      }
      addMessage(newMessage)
    })

    connection.onclose(() => {
      console.log('[ChatService] SignalR 連線已關閉')
      setConnectionStatus('連線已關閉', false)
    })

    connection.onreconnecting(() => {
      console.log('[ChatService] SignalR 重新連線中...')
      setConnectionStatus('重新連線中...', false)
    })

    connection.onreconnected(() => {
      console.log('[ChatService] SignalR 已重新連線')
      setConnectionStatus('已連線', true)
    })

    connectionRef.current = connection
    return connection
  }, [addMessage, setConnectionStatus])

  const connect = useCallback(async () => {
    const connection = createConnection()

    try {
      setConnectionStatus('連線中...', false)
      await connection.start()
      console.log('[ChatService] SignalR 連線已建立')
      setConnectionStatus('已連線', true)
      retryCountRef.current = 0

      // 取得連線ID
      try {
        const connectionId = await connection.invoke(SIGNALR_EVENTS.GET_CONNECTION_ID)
        console.log('[ChatService] 連線ID:', connectionId)
      } catch (error) {
        console.warn('[ChatService] 無法取得連線ID:', error)
      }
    } catch (error) {
      console.error('[ChatService] SignalR 連線失敗:', error)
      setConnectionStatus('連線失敗', false)

      // 重試邏輯
      if (retryCountRef.current < 5) {
        retryCountRef.current++
        setTimeout(() => connect(), 5000)
      }
    }
  }, [createConnection, setConnectionStatus])

  const disconnect = useCallback(async () => {
    if (connectionRef.current) {
      try {
        await connectionRef.current.stop()
        console.log('[ChatService] SignalR 連線已停止')
      } catch (error) {
        console.error('[ChatService] 停止連線時發生錯誤:', error)
      } finally {
        connectionRef.current = null
        setConnectionStatus('未連線', false)
      }
    }
  }, [setConnectionStatus])

  const sendMessage = useCallback(async (user: string, message: string) => {
    const connection = connectionRef.current
    if (!connection || connection.state !== signalR.HubConnectionState.Connected) {
      throw new Error('SignalR 連線未建立')
    }

    try {
      await connection.invoke(SIGNALR_EVENTS.SEND_MESSAGE, user, message)
    } catch (error) {
      console.error('[ChatService] 發送訊息失敗:', error)
      throw error
    }
  }, [])

  const getConnectionState = useCallback(() => {
    return connectionRef.current?.state || signalR.HubConnectionState.Disconnected
  }, [])

  const isConnected = useCallback(() => {
    return connectionRef.current?.state === signalR.HubConnectionState.Connected
  }, [])

  // 組件掛載時自動連線，卸載時自動斷線
  useEffect(() => {
    connect()
    return () => {
      disconnect()
    }
  }, [connect, disconnect])

  return {
    sendMessage,
    connect,
    disconnect,
    getConnectionState,
    isConnected,
  }
}
