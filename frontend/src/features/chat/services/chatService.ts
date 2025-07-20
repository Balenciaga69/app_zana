// SignalR 聊天服務
import * as signalR from '@microsoft/signalr'
import { config } from '@/config/config'

export class ChatService {
  private connection: signalR.HubConnection | null = null
  private onMessageReceived: ((user: string, message: string) => void) | null = null

  constructor() {
    // 建立 SignalR 連線
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      .withAutomaticReconnect()
      .build()

    this.setupEventHandlers()
  }

  private setupEventHandlers() {
    if (!this.connection) return

    // 監聽來自後端的訊息
    this.connection.on('ReceiveMessage', (user: string, message: string) => {
      console.log('收到訊息:', user, message)
      if (this.onMessageReceived) {
        this.onMessageReceived(user, message)
      }
    })

    // 連線狀態變更
    this.connection.onclose(() => {
      console.log('SignalR 連線已關閉')
    })

    this.connection.onreconnecting(() => {
      console.log('SignalR 重新連線中...')
    })

    this.connection.onreconnected(() => {
      console.log('SignalR 已重新連線')
    })
  }

  // 開始連線
  public async start(): Promise<void> {
    if (!this.connection) return

    try {
      await this.connection.start()
      console.log('SignalR 連線已建立')

      // 取得連線ID（可選）
      const connectionId = await this.connection.invoke('GetConnectionId')
      console.log('連線ID:', connectionId)
    } catch (error) {
      console.error('SignalR 連線失敗:', error)
      // 5秒後重試
      setTimeout(() => this.start(), 5000)
    }
  }

  // 停止連線
  public async stop(): Promise<void> {
    if (this.connection) {
      await this.connection.stop()
      console.log('SignalR 連線已停止')
    }
  }

  // 發送訊息到後端
  public async sendMessage(user: string, message: string): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.connection.invoke('SendMessage', user, message)
      } catch (error) {
        console.error('發送訊息失敗:', error)
      }
    } else {
      console.error('SignalR 連線未建立')
    }
  }

  // 設定訊息接收回調
  public setMessageHandler(handler: (user: string, message: string) => void) {
    this.onMessageReceived = handler
  }

  // 取得連線狀態
  public get connectionState(): signalR.HubConnectionState | undefined {
    return this.connection?.state
  }

  // 檢查是否已連線
  public get isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected
  }
}

// 建立單例服務實例
export const chatService = new ChatService()
