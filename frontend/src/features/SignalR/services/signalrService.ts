import { config } from '@/Shared/config'
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { type SignalREvent, SignalREvents } from '../models/SignalREvents'

type EventCallback = (payload: any) => void

class SignalRService {
  // 單例實例
  private static instance: SignalRService
  // SignalR 連接實例
  private connection: HubConnection | null = null
  // 事件監聽器集合
  private listeners: Map<SignalREvent, Set<EventCallback>> = new Map()

  private constructor() {}

  // 取得單例實例
  public static getInstance(): SignalRService {
    if (!SignalRService.instance) {
      SignalRService.instance = new SignalRService()
    }
    return SignalRService.instance
  }

  // 建立 SignalR 連線並註冊所有事件
  public async connect(): Promise<void> {
    if (this.connection) return

    this.connection = new HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build()

    // 自動註冊所有 SignalREvents 事件
    Object.values(SignalREvents).forEach((event) => {
      this.connection!.on(event, (payload) => {
        this.emit(event, payload)
      })
    })
    // ...如有其他事件請自行補上

    await this.connection.start()
  }

  // 中斷 SignalR 連線並清理事件
  public async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop()
      this.connection = null
    }
    this.listeners.clear()
  }

  /**
   * 註冊事件監聽 callback
   */
  public on(event: SignalREvent, callback: EventCallback): void {
    if (!this.listeners.has(event)) {
      this.listeners.set(event, new Set())
    }
    this.listeners.get(event)!.add(callback)
  }

  /**
   * 移除事件監聽 callback
   */
  public off(event: SignalREvent, callback: EventCallback): void {
    if (this.listeners.has(event)) {
      this.listeners.get(event)!.delete(callback)
      if (this.listeners.get(event)!.size === 0) {
        this.listeners.delete(event)
      }
    }
  }

  /**
   * 發送訊息到 SignalR Hub
   */
  public async invoke(event: SignalREvent, payload: any): Promise<void> {
    if (!this.connection) throw new Error('SignalR not connected')
    await this.connection.invoke(event, payload)
  }

  /**
   * 觸發所有訂閱該事件的 callback
   */
  private emit(event: SignalREvent, payload: any): void {
    if (!this.listeners.has(event)) return
    for (const eventCallBack of this.listeners.get(event)!) {
      try {
        eventCallBack(payload)
      } catch {
        // @Balenciaga69 暫時沒想到要怎麼處理錯誤
      }
    }
  }
}

export default SignalRService
