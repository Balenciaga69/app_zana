import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { config } from '../config/config'

export type ConnectionState = 'disconnected' | 'connecting' | 'connected' | 'reconnecting' | 'error'

export class SignalRService {
  private connection: HubConnection | null = null
  private maxReconnectAttempts: number = 5
  private reconnectDelay: number = 3000

  async connect(): Promise<HubConnection> {
    if (this.connection?.state === 'Connected') {
      return this.connection
    }

    this.connection = new HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      // 設定自動重連
      .withAutomaticReconnect({
        // 設定重連次數和延遲
        nextRetryDelayInMilliseconds: (retryContext) => {
          // 如果重試次數 < 最大重試次數，則返回重連延遲
          if (retryContext.previousRetryCount < this.maxReconnectAttempts) {
            return this.reconnectDelay
          }
          return null // 停止重連
        },
      })
      .configureLogging(LogLevel.Information)
      .build()
    this.setupEventListeners()
    await this.connection.start()
    return this.connection
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
    })
    // 可擴充事件監聽
  }
}

export const signalRService = new SignalRService()
