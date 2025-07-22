// Redux Reducer
import { CHAT_ACTION_TYPES } from './actionTypes'

// State 介面定義
interface ChatState {
  // 使用者相關
  currentUser: string
  userProfile: any | null
  
  // 房間相關
  roomStats: any | null
  roomStatsLoading: boolean
  roomStatsError: string | null
  
  // 訊息相關
  messages: any[]
  users: string[]
  sendingMessage: boolean
  sendMessageError: string | null
  
  // 點數相關
  chatPoints: number
  pointsLoading: boolean
  pointsError: string | null
  purchasingPoints: boolean
  purchaseError: string | null
  usingPoints: boolean
  usePointsError: string | null
  
  // SignalR 連線相關
  isConnected: boolean
  connectionStatus: string
  connectionId: string | null
  connecting: boolean
  connectionError: string | null
  
  // UI 相關
  toast: {
    visible: boolean
    message: string
    type: 'success' | 'error' | 'warning'
  } | null
  
  // 載入狀態
  loading: {
    nickname: boolean
    roomStats: boolean
    points: boolean
    purchase: boolean
    usePoints: boolean
  }
}

const initialState: ChatState = {
  currentUser: '',
  userProfile: null,
  
  roomStats: null,
  roomStatsLoading: false,
  roomStatsError: null,
  
  messages: [],
  users: [],
  sendingMessage: false,
  sendMessageError: null,
  
  chatPoints: 0,
  pointsLoading: false,
  pointsError: null,
  purchasingPoints: false,
  purchaseError: null,
  usingPoints: false,
  usePointsError: null,
  
  isConnected: false,
  connectionStatus: '未連線',
  connectionId: null,
  connecting: false,
  connectionError: null,
  
  toast: null,
  
  loading: {
    nickname: false,
    roomStats: false,
    points: false,
    purchase: false,
    usePoints: false,
  }
}

export function chatReducer(state = initialState, action: any): ChatState {
  switch (action.type) {
    // 房間統計
    case CHAT_ACTION_TYPES.FETCH_ROOM_STATS_REQUEST:
      return {
        ...state,
        roomStatsLoading: true,
        roomStatsError: null,
        loading: { ...state.loading, roomStats: true }
      }
    
    case CHAT_ACTION_TYPES.FETCH_ROOM_STATS_SUCCESS:
      return {
        ...state,
        roomStats: action.payload,
        roomStatsLoading: false,
        roomStatsError: null,
        loading: { ...state.loading, roomStats: false }
      }
    
    case CHAT_ACTION_TYPES.FETCH_ROOM_STATS_FAILURE:
      return {
        ...state,
        roomStatsLoading: false,
        roomStatsError: action.payload.error,
        loading: { ...state.loading, roomStats: false }
      }

    // 更新暱稱
    case CHAT_ACTION_TYPES.UPDATE_NICKNAME_REQUEST:
      return {
        ...state,
        loading: { ...state.loading, nickname: true }
      }
    
    case CHAT_ACTION_TYPES.UPDATE_NICKNAME_SUCCESS:
      return {
        ...state,
        userProfile: action.payload,
        loading: { ...state.loading, nickname: false }
      }
    
    case CHAT_ACTION_TYPES.UPDATE_NICKNAME_FAILURE:
      return {
        ...state,
        loading: { ...state.loading, nickname: false }
      }

    // 購買點數
    case CHAT_ACTION_TYPES.PURCHASE_POINTS_REQUEST:
      return {
        ...state,
        purchasingPoints: true,
        purchaseError: null,
        loading: { ...state.loading, purchase: true }
      }
    
    case CHAT_ACTION_TYPES.PURCHASE_POINTS_SUCCESS:
      return {
        ...state,
        chatPoints: action.payload.newBalance,
        purchasingPoints: false,
        purchaseError: null,
        loading: { ...state.loading, purchase: false }
      }
    
    case CHAT_ACTION_TYPES.PURCHASE_POINTS_FAILURE:
      return {
        ...state,
        purchasingPoints: false,
        purchaseError: action.payload.error,
        loading: { ...state.loading, purchase: false }
      }

    // 使用點數
    case CHAT_ACTION_TYPES.USE_POINTS_REQUEST:
      return {
        ...state,
        usingPoints: true,
        usePointsError: null,
        loading: { ...state.loading, usePoints: true }
      }
    
    case CHAT_ACTION_TYPES.USE_POINTS_SUCCESS:
      return {
        ...state,
        chatPoints: action.payload.newBalance,
        usingPoints: false,
        usePointsError: null,
        loading: { ...state.loading, usePoints: false }
      }
    
    case CHAT_ACTION_TYPES.USE_POINTS_FAILURE:
      return {
        ...state,
        usingPoints: false,
        usePointsError: action.payload.error,
        loading: { ...state.loading, usePoints: false }
      }

    // SignalR 連線
    case CHAT_ACTION_TYPES.SIGNALR_CONNECT_REQUEST:
      return {
        ...state,
        connecting: true,
        connectionError: null
      }
    
    case CHAT_ACTION_TYPES.SIGNALR_CONNECT_SUCCESS:
      return {
        ...state,
        isConnected: true,
        connectionId: action.payload.connectionId,
        connecting: false,
        connectionError: null
      }
    
    case CHAT_ACTION_TYPES.SIGNALR_CONNECT_FAILURE:
      return {
        ...state,
        isConnected: false,
        connecting: false,
        connectionError: action.payload.error
      }
    
    case CHAT_ACTION_TYPES.SIGNALR_DISCONNECT:
      return {
        ...state,
        isConnected: false,
        connectionId: null,
        connectionStatus: '未連線'
      }

    // 訊息處理
    case CHAT_ACTION_TYPES.SEND_MESSAGE_REQUEST:
      return {
        ...state,
        sendingMessage: true,
        sendMessageError: null
      }
    
    case CHAT_ACTION_TYPES.SEND_MESSAGE_SUCCESS:
      return {
        ...state,
        sendingMessage: false,
        sendMessageError: null
      }
    
    case CHAT_ACTION_TYPES.SEND_MESSAGE_FAILURE:
      return {
        ...state,
        sendingMessage: false,
        sendMessageError: action.payload.error
      }
    
    case CHAT_ACTION_TYPES.RECEIVE_MESSAGE:
      const newMessage = {
        user: action.payload.user,
        text: action.payload.message,
        timestamp: action.payload.timestamp
      }
      return {
        ...state,
        messages: [...state.messages, newMessage],
        users: state.users.includes(action.payload.user) 
          ? state.users 
          : [...state.users, action.payload.user]
      }

    // UI 狀態
    case CHAT_ACTION_TYPES.SET_CURRENT_USER:
      return {
        ...state,
        currentUser: action.payload.username
      }
    
    case CHAT_ACTION_TYPES.SET_CONNECTION_STATUS:
      return {
        ...state,
        connectionStatus: action.payload.status,
        isConnected: action.payload.connected
      }
    
    case CHAT_ACTION_TYPES.SHOW_TOAST:
      return {
        ...state,
        toast: {
          visible: true,
          message: action.payload.message,
          type: action.payload.type
        }
      }
    
    case CHAT_ACTION_TYPES.HIDE_TOAST:
      return {
        ...state,
        toast: null
      }

    default:
      return state
  }
}
