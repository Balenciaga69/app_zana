// SignalR 聊天服務
import * as signalR from '@microsoft/signalr'
import { config } from '@/config/config'

// 事件名稱常數
const SIGNALR_EVENTS = {
  RECEIVE_MESSAGE: 'ReceiveMessage',
  SEND_MESSAGE: 'SendMessage',
  GET_CONNECTION_ID: 'GetConnectionId',
}

// 簡易 log utility
const log = {
  info: (...args: any[]) => console.log('[ChatService]', ...args),
  error: (...args: any[]) => console.error('[ChatService]', ...args),
}

export class ChatService {
  private readonly connection: signalR.HubConnection
  private onMessageReceived: ((user: string, message: string) => void) | null = null
  private retryCount = 0
  private readonly maxRetries = 5
  private readonly retryDelay = 5000

  constructor() {
    // 建立 SignalR 連線
    this.connection = new signalR.HubConnectionBuilder().withUrl(config.signalR.hubUrl).withAutomaticReconnect().build()
    this.setupEventHandlers()
  }

  private setupEventHandlers() {
    // 監聽來自後端的訊息
    this.connection.on(SIGNALR_EVENTS.RECEIVE_MESSAGE, (user: string, message: string) => {
      log.info('收到訊息:', user, message)
      if (this.onMessageReceived) {
        this.onMessageReceived(user, message)
      }
    })

    // 連線狀態變更
    this.connection.onclose(() => {
      log.info('SignalR 連線已關閉')
    })
    this.connection.onreconnecting(() => {
      log.info('SignalR 重新連線中...')
    })
    this.connection.onreconnected(() => {
      log.info('SignalR 已重新連線')
    })
  }

  // 解除事件綁定（避免記憶體洩漏）
  private removeEventHandlers() {
    this.connection.off(SIGNALR_EVENTS.RECEIVE_MESSAGE)
    this.connection.off('close')
    this.connection.off('reconnecting')
    this.connection.off('reconnected')
  }

  // 開始連線
  public async start(): Promise<void> {
    try {
      await this.connection.start()
      log.info('SignalR 連線已建立')
      this.retryCount = 0
      // 取得連線ID（可選）
      const connectionId = await this.connection.invoke(SIGNALR_EVENTS.GET_CONNECTION_ID)
      log.info('連線ID:', connectionId)
    } catch (error) {
      log.error('SignalR 連線失敗:', error)
      if (this.retryCount < this.maxRetries) {
        this.retryCount++
        setTimeout(() => this.start(), this.retryDelay)
      } else {
        log.error('已達最大重試次數，請檢查網路或伺服器狀態。')
      }
    }
  }

  // 停止連線
  public async stop(): Promise<void> {
    await this.connection.stop()
    this.removeEventHandlers()
    log.info('SignalR 連線已停止')
  }

  // 發送訊息到後端
  public async sendMessage(user: string, message: string): Promise<void> {
    if (this.connection.state === signalR.HubConnectionState.Connected) {
      try {
        await this.connection.invoke(SIGNALR_EVENTS.SEND_MESSAGE, user, message)
      } catch (error) {
        log.error('發送訊息失敗:', error)
      }
    } else {
      log.error('SignalR 連線未建立')
    }
  }

  // 設定訊息接收回調
  public setMessageHandler(handler: (user: string, message: string) => void) {
    this.onMessageReceived = handler
  }

  // 取得連線狀態
  public get connectionState(): signalR.HubConnectionState {
    return this.connection.state
  }

  // 檢查是否已連線
  public get isConnected(): boolean {
    return this.connection.state === signalR.HubConnectionState.Connected
  }
}

// 建立單例服務實例
export const chatService = new ChatService()
