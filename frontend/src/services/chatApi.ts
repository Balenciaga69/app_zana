// 聊天室相關的 API 服務
import { apiClient, createApiCall } from './apiClient'

// 類型定義
export interface RoomStats {
  roomId: string
  userCount: number
  activeUsers: string[]
  messageCount: number
}

export interface UserProfile {
  id: string
  nickname: string
  avatar?: string
  chatPoints: number
  level: number
}

export interface PurchasePointsRequest {
  amount: number
  paymentMethod: string
}

export interface PurchasePointsResponse {
  success: boolean
  newBalance: number
  transactionId: string
}

// API 呼叫函數
export const chatApi = {
  // 取得房間統計資料
  getRoomStats: createApiCall<RoomStats, { roomId: string }>((params) => ({
    method: 'GET',
    url: `/chat/rooms/${params.roomId}/stats`,
  })),

  // 更新使用者暱稱
  updateNickname: createApiCall<UserProfile, { nickname: string }>((params) => ({
    method: 'PUT',
    url: '/users/profile/nickname',
    data: { nickname: params.nickname },
  })),

  // 取得使用者資料
  getUserProfile: createApiCall<UserProfile, { userId?: string }>((params) => ({
    method: 'GET',
    url: params.userId ? `/users/${params.userId}` : '/users/me',
  })),

  // 購買聊天點數
  purchaseChatPoints: createApiCall<PurchasePointsResponse, PurchasePointsRequest>((params) => ({
    method: 'POST',
    url: '/payments/chat-points',
    data: params,
  })),

  // 取得聊天點數餘額
  getChatPointsBalance: createApiCall<{ balance: number }, {}>(() => ({
    method: 'GET',
    url: '/users/chat-points',
  })),

  // 使用聊天點數 (例如發送特殊訊息)
  useChatPoints: createApiCall<{ success: boolean; newBalance: number }, { points: number; action: string }>((params) => ({
    method: 'POST',
    url: '/users/chat-points/use',
    data: params,
  })),
}
