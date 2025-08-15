import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { config } from '../config/config'

export type ConnectionState = 'disconnected' | 'connecting' | 'connected' | 'reconnecting' | 'error'

export class SignalRService {
  private connection: HubConnection | null = null
  private reconnectAttempts = 0
  private maxReconnectAttempts = 5
  private reconnectDelay = 3000

  async connect(): Promise<HubConnection> {
    if (this.connection?.state === 'Connected') {
      return this.connection
    }

    this.connection = new HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          if (retryContext.previousRetryCount < this.maxReconnectAttempts) {
            return this.reconnectDelay
          }
          return null // 停止重連
        },
      })
      .configureLogging(LogLevel.Information)
      .build()

    // 註冊事件監聽器
    this.setupEventListeners()

    try {
      await this.connection.start()
      this.reconnectAttempts = 0
      console.log('SignalR Connected successfully')
      return this.connection
    } catch (error) {
      console.error('SignalR Connection failed:', error)
      throw error
    }
  }

  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop()
      this.connection = null
    }
  }

  getConnection(): HubConnection | null {
    return this.connection
  }

  getConnectionState(): ConnectionState {
    if (!this.connection) return 'disconnected'

    switch (this.connection.state) {
      case 'Connected':
        return 'connected'
      case 'Connecting':
        return 'connecting'
      case 'Reconnecting':
        return 'reconnecting'
      case 'Disconnected':
        return 'disconnected'
      default:
        return 'error'
    }
  }

  private setupEventListeners(): void {
    if (!this.connection) return

    this.connection.onclose((error) => {
      console.log('SignalR Connection closed', error)
    })

    this.connection.onreconnecting((error) => {
      console.log('SignalR Reconnecting...', error)
    })

    this.connection.onreconnected((connectionId) => {
      console.log('SignalR Reconnected', connectionId)
      this.reconnectAttempts = 0
    })

    // TODO: 後續添加聊天相關的事件監聽
    // this.connection.on('ReceiveMessage', (message) => {
    //   // 處理接收到的訊息
    // })
  }
}

// 單例模式
export const signalRService = new SignalRService()
