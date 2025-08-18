/**
 * 集中管理所有 SignalR 事件名稱
 */
export const SignalREvents = {
  MESSAGE_RECEIVED: 'messageReceived',
  NICKNAME_UPDATED: 'NicknameUpdated',
  REGISTER_USER: 'RegisterUser',
  // ...可持續擴充
} as const

/**
 * SignalR 事件類型
 * 用於定義所有可能的 SignalR 事件名稱
 */
export type SignalREvent = (typeof SignalREvents)[keyof typeof SignalREvents]
