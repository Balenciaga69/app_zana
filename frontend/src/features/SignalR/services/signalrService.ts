import { config } from '@/Shared/config'
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { type SignalREvent, SignalREvents } from '../models/SignalREvents'

type EventCallback = (...arg: any[]) => void

class SignalRService {
  // 單例實例
  private static instance: SignalRService
  // SignalR 連接實例
  private connection: HubConnection | null = null
  // 事件監聽器集合
  private listeners: Map<SignalREvent, Set<EventCallback>> = new Map()
  // 最後註冊的裝置指紋
  private lastRegisteredDeviceFingerprint?: string

  private constructor() {}

  // 取得單例實例
  public static getInstance(): SignalRService {
    if (!SignalRService.instance) {
      SignalRService.instance = new SignalRService()
    }
    return SignalRService.instance
  }

  // 建立 SignalR 連線並註冊所有事件，含 retry/backoff
  public async connect(): Promise<void> {
    if (this.connection) return
    this.connection = new HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build()
    this.registerAllEvents()
    await this.retryAsync(() => this.connection!.start(), 5, 500)
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
      // 移除指定的 callback
      this.listeners.get(event)!.delete(callback)
      // 當該 event 沒有任何 callback 就移除整個 event entry
      if (this.listeners.get(event)!.size === 0) {
        this.listeners.delete(event)
      }
    }
  }

  /**
   * 發送訊息到 SignalR Hub
   */
  public async invoke(event: SignalREvent, ...args: unknown[]): Promise<void> {
    if (!this.connection) throw new Error('SignalR not connected')
    await this.retryAsync(() => this.connection!.invoke(event, ...args), 3, 200)
  }

  /**
   * 註冊用戶，含前端驗證與錯誤導流
   */
  public async registerUser(deviceFingerprint: string): Promise<void> {
    if (!this.connection) throw new Error('SignalR not connected')
    if (!deviceFingerprint || deviceFingerprint.length < 32 || deviceFingerprint.length > 128) {
      throw new Error('Invalid device fingerprint')
    }
    try {
      this.lastRegisteredDeviceFingerprint = deviceFingerprint
      await this.connection.invoke(SignalREvents.REGISTER_USER, deviceFingerprint)
    } catch (err: unknown) {
      if (
        // 檢查錯誤是否為物件
        typeof err === 'object' &&
        // 檢查錯誤不為 null
        err !== null &&
        // 檢查錯誤物件是否有 message 屬性
        'message' in err &&
        // 檢查 message 屬性是否為字串
        typeof (err as { message: unknown }).message === 'string' &&
        // 檢查 message 是否包含 aborted 字串
        (err as { message: string }).message.includes('aborted')
      ) {
        // 連線中斷時註冊失敗
        throw new Error('RegisterUser failed: connection aborted')
      }
      throw err
    }
  }

  // 釋放所有事件與連線
  public async dispose(): Promise<void> {
    if (this.connection) {
      this.unregisterAllEvents()
      await this.connection.stop()
      this.connection = null
    }
    this.listeners.clear()
    this.lastRegisteredDeviceFingerprint = undefined
  }

  /**
   * retryAsync: 重試函數，支援 exponential backoff
   */
  private async retryAsync<T>(fn: () => Promise<T>, maxAttempts = 3, baseDelay = 200): Promise<T> {
    let attempt = 0
    while (attempt < maxAttempts) {
      try {
        return await fn()
      } catch (err) {
        attempt++
        if (attempt >= maxAttempts) throw err
        await new Promise((r) => setTimeout(r, baseDelay * Math.pow(2, attempt - 1)))
      }
    }
    throw new Error('retryAsync: should not reach here')
  }

  /**
   * registerAllEvents: 註冊所有 SignalR 事件
   */
  private registerAllEvents(): void {
    if (!this.connection) return

    // 走訪所有事件並註冊
    Object.values(SignalREvents).forEach((event) => {
      this.connection!.off(event)
      this.connection!.on(event, (payload) => {
        this.emit(event, payload)
      })
    })
    this.connection.onclose(() => {})
    this.connection.onreconnecting(() => {})
    this.connection.onreconnected(() => this.reRegisterUserIfNeeded())
  }

  /**
   * reRegisterUserIfNeeded: 如果有最後註冊的裝置指紋，則重新註冊用戶
   */
  private async reRegisterUserIfNeeded(): Promise<void> {
    if (!this.lastRegisteredDeviceFingerprint) return
    await this.retryAsync(() => this.registerUser(this.lastRegisteredDeviceFingerprint!), 3, 500)
  }

  /**
   * unregisterAllEvents: 移除所有 SignalR 事件
   */
  private unregisterAllEvents(): void {
    if (!this.connection) return
    Object.values(SignalREvents).forEach((event) => {
      this.connection!.off(event)
    })
  }

  /**
   * 觸發所有訂閱該事件的 callback
   */
  private emit(event: SignalREvent, ...args: unknown[]): void {
    if (!this.listeners.has(event)) return
    for (const eventCallBack of this.listeners.get(event)!) {
      try {
        eventCallBack(...args)
      } catch {
        // @Balenciaga69 暫時沒想到要怎麼處理錯誤
      }
    }
  }
}

// 簡單解釋: 當後端發送事件時，會觸發所有註冊該事件的 callback (listeners 集合)
export default SignalRService
