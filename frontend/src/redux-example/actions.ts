// Redux Action Creators
import { CHAT_ACTION_TYPES } from './actionTypes'

// 房間統計 Actions
export const fetchRoomStatsRequest = (roomId: string) => ({
  type: CHAT_ACTION_TYPES.FETCH_ROOM_STATS_REQUEST,
  payload: { roomId }
})

export const fetchRoomStatsSuccess = (roomStats: any) => ({
  type: CHAT_ACTION_TYPES.FETCH_ROOM_STATS_SUCCESS,
  payload: roomStats
})

export const fetchRoomStatsFailure = (error: string) => ({
  type: CHAT_ACTION_TYPES.FETCH_ROOM_STATS_FAILURE,
  payload: { error }
})

// 更新暱稱 Actions
export const updateNicknameRequest = (nickname: string) => ({
  type: CHAT_ACTION_TYPES.UPDATE_NICKNAME_REQUEST,
  payload: { nickname }
})

export const updateNicknameSuccess = (profile: any) => ({
  type: CHAT_ACTION_TYPES.UPDATE_NICKNAME_SUCCESS,
  payload: profile
})

export const updateNicknameFailure = (error: string) => ({
  type: CHAT_ACTION_TYPES.UPDATE_NICKNAME_FAILURE,
  payload: { error }
})

// 購買點數 Actions
export const purchasePointsRequest = (amount: number, paymentMethod: string) => ({
  type: CHAT_ACTION_TYPES.PURCHASE_POINTS_REQUEST,
  payload: { amount, paymentMethod }
})

export const purchasePointsSuccess = (response: any) => ({
  type: CHAT_ACTION_TYPES.PURCHASE_POINTS_SUCCESS,
  payload: response
})

export const purchasePointsFailure = (error: string) => ({
  type: CHAT_ACTION_TYPES.PURCHASE_POINTS_FAILURE,
  payload: { error }
})

// 使用點數 Actions
export const usePointsRequest = (points: number, action: string) => ({
  type: CHAT_ACTION_TYPES.USE_POINTS_REQUEST,
  payload: { points, action }
})

export const usePointsSuccess = (response: any) => ({
  type: CHAT_ACTION_TYPES.USE_POINTS_SUCCESS,
  payload: response
})

export const usePointsFailure = (error: string) => ({
  type: CHAT_ACTION_TYPES.USE_POINTS_FAILURE,
  payload: { error }
})

// SignalR Actions
export const signalrConnectRequest = () => ({
  type: CHAT_ACTION_TYPES.SIGNALR_CONNECT_REQUEST
})

export const signalrConnectSuccess = (connectionId: string) => ({
  type: CHAT_ACTION_TYPES.SIGNALR_CONNECT_SUCCESS,
  payload: { connectionId }
})

export const signalrConnectFailure = (error: string) => ({
  type: CHAT_ACTION_TYPES.SIGNALR_CONNECT_FAILURE,
  payload: { error }
})

export const signalrDisconnect = () => ({
  type: CHAT_ACTION_TYPES.SIGNALR_DISCONNECT
})

// 訊息 Actions
export const sendMessageRequest = (user: string, message: string) => ({
  type: CHAT_ACTION_TYPES.SEND_MESSAGE_REQUEST,
  payload: { user, message }
})

export const sendMessageSuccess = () => ({
  type: CHAT_ACTION_TYPES.SEND_MESSAGE_SUCCESS
})

export const sendMessageFailure = (error: string) => ({
  type: CHAT_ACTION_TYPES.SEND_MESSAGE_FAILURE,
  payload: { error }
})

export const receiveMessage = (user: string, message: string, timestamp: number) => ({
  type: CHAT_ACTION_TYPES.RECEIVE_MESSAGE,
  payload: { user, message, timestamp }
})

// UI Actions
export const setCurrentUser = (username: string) => ({
  type: CHAT_ACTION_TYPES.SET_CURRENT_USER,
  payload: { username }
})

export const setConnectionStatus = (status: string, connected: boolean) => ({
  type: CHAT_ACTION_TYPES.SET_CONNECTION_STATUS,
  payload: { status, connected }
})

export const showToast = (message: string, type: 'success' | 'error' | 'warning') => ({
  type: CHAT_ACTION_TYPES.SHOW_TOAST,
  payload: { message, type }
})

export const hideToast = () => ({
  type: CHAT_ACTION_TYPES.HIDE_TOAST
})
