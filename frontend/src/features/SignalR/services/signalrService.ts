import { config } from '@/Shared/config/config'
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export type ConnectionState = 'disconnected' | 'connecting' | 'connected' | 'reconnecting' | 'error'

/**
 * SignalR Service
 * 用於管理 SignalR 連接和事件
 */
export class SignalRService {
  private connection: HubConnection | null = null
  private maxReconnectAttempts: number = 5
  private reconnectDelay: number = 3000

  /**
   * 建立 SignalR 連接
   */
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

  /**
   * 斷開 SignalR 連接
   */
  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop()
      this.connection = null
    }
  }

  /**
   * 獲取 SignalR 連接實例
   */
  getConnection(): HubConnection | null {
    return this.connection
  }

  /**
   * 設定 SignalR 事件監聽器
   */
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

  /**
   * TODO: 註冊/重新連線用戶
   * 之後這裡要呼叫 SignalR Hub 的 RegisterUser(deviceFingerprint)
   * 並監聽 UserRegistered/ConnectionEstablished 事件
   */
  async registerUser(deviceFingerprint: string): Promise<void> {
    // TODO: 實作 deviceFingerprint 註冊流程
  }
}

export const signalRService = new SignalRService()
