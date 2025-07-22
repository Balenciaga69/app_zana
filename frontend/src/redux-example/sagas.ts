// Redux-Saga: 處理副作用 (API 呼叫、SignalR 連線等)
import { call, put, take, fork, cancel, cancelled, takeEvery, takeLatest, select } from 'redux-saga/effects'
import { eventChannel, EventChannel } from 'redux-saga'
import { CHAT_ACTION_TYPES } from './actionTypes'
import * as actions from './actions'
import { chatApi } from '../services/chatApi'
import * as signalR from '@microsoft/signalr'
import { config } from '../config/config'

// API 呼叫的 Saga
function* fetchRoomStatsSaga(action: ReturnType<typeof actions.fetchRoomStatsRequest>) {
  try {
    const roomStats = yield call(chatApi.getRoomStats, { roomId: action.payload.roomId })
    yield put(actions.fetchRoomStatsSuccess(roomStats))
  } catch (error) {
    yield put(actions.fetchRoomStatsFailure(error.message))
    yield put(actions.showToast('載入房間統計失敗', 'error'))
  }
}

function* updateNicknameSaga(action: ReturnType<typeof actions.updateNicknameRequest>) {
  try {
    const profile = yield call(chatApi.updateNickname, { nickname: action.payload.nickname })
    yield put(actions.updateNicknameSuccess(profile))
    yield put(actions.setCurrentUser(profile.nickname))
    yield put(actions.showToast('暱稱更新成功', 'success'))
  } catch (error) {
    yield put(actions.updateNicknameFailure(error.message))
    yield put(actions.showToast('暱稱更新失敗', 'error'))
  }
}

function* purchasePointsSaga(action: ReturnType<typeof actions.purchasePointsRequest>) {
  try {
    const response = yield call(chatApi.purchaseChatPoints, {
      amount: action.payload.amount,
      paymentMethod: action.payload.paymentMethod
    })
    yield put(actions.purchasePointsSuccess(response))
    yield put(actions.showToast(`購買成功！新餘額: ${response.newBalance}`, 'success'))
    
    // 重新載入點數餘額
    yield put({ type: CHAT_ACTION_TYPES.FETCH_POINTS_BALANCE_REQUEST })
  } catch (error) {
    yield put(actions.purchasePointsFailure(error.message))
    yield put(actions.showToast('購買失敗', 'error'))
  }
}

function* usePointsSaga(action: ReturnType<typeof actions.usePointsRequest>) {
  try {
    const response = yield call(chatApi.useChatPoints, {
      points: action.payload.points,
      action: action.payload.action
    })
    yield put(actions.usePointsSuccess(response))
    yield put(actions.showToast(`使用 ${action.payload.points} 點數成功`, 'success'))
    
    // 重新載入點數餘額
    yield put({ type: CHAT_ACTION_TYPES.FETCH_POINTS_BALANCE_REQUEST })
  } catch (error) {
    yield put(actions.usePointsFailure(error.message))
    yield put(actions.showToast('使用點數失敗', 'error'))
  }
}

// SignalR 連線的 Saga
function createSignalRChannel(connection: signalR.HubConnection): EventChannel<any> {
  return eventChannel(emit => {
    // 監聽訊息
    connection.on('ReceiveMessage', (user: string, message: string) => {
      emit(actions.receiveMessage(user, message, Date.now()))
    })

    // 監聽連線狀態
    connection.onclose(() => {
      emit(actions.setConnectionStatus('連線已關閉', false))
    })

    connection.onreconnecting(() => {
      emit(actions.setConnectionStatus('重新連線中...', false))
    })

    connection.onreconnected(() => {
      emit(actions.setConnectionStatus('已連線', true))
    })

    // 清理函數
    return () => {
      connection.off('ReceiveMessage')
    }
  })
}

function* signalRConnectionSaga() {
  let signalRChannel: EventChannel<any> | null = null
  let connection: signalR.HubConnection | null = null

  try {
    // 建立連線
    connection = new signalR.HubConnectionBuilder()
      .withUrl(config.signalR.hubUrl)
      .withAutomaticReconnect()
      .build()

    yield put(actions.setConnectionStatus('連線中...', false))
    yield call([connection, 'start'])
    
    // 連線成功
    const connectionId = yield call([connection, 'invoke'], 'GetConnectionId')
    yield put(actions.signalrConnectSuccess(connectionId))
    yield put(actions.setConnectionStatus('已連線', true))

    // 建立事件頻道
    signalRChannel = yield call(createSignalRChannel, connection)

    // 監聽頻道事件
    while (true) {
      const action = yield take(signalRChannel)
      yield put(action)
    }

  } catch (error) {
    yield put(actions.signalrConnectFailure(error.message))
    yield put(actions.setConnectionStatus('連線失敗', false))
  } finally {
    if (yield cancelled()) {
      // 清理資源
      if (signalRChannel) {
        signalRChannel.close()
      }
      if (connection) {
        yield call([connection, 'stop'])
      }
    }
  }
}

function* sendMessageSaga(action: ReturnType<typeof actions.sendMessageRequest>) {
  try {
    // 取得目前的連線狀態
    const isConnected = yield select((state: any) => state.chat.isConnected)
    
    if (!isConnected) {
      throw new Error('SignalR 未連線')
    }

    // 這裡需要從某處取得 connection 實例
    // 在實際應用中，可能需要將 connection 存在 Redux store 或使用其他方式
    yield put(actions.sendMessageSuccess())
  } catch (error) {
    yield put(actions.sendMessageFailure(error.message))
    yield put(actions.showToast('發送訊息失敗', 'error'))
  }
}

// Root Saga
export function* chatSaga() {
  yield takeEvery(CHAT_ACTION_TYPES.FETCH_ROOM_STATS_REQUEST, fetchRoomStatsSaga)
  yield takeEvery(CHAT_ACTION_TYPES.UPDATE_NICKNAME_REQUEST, updateNicknameSaga)
  yield takeEvery(CHAT_ACTION_TYPES.PURCHASE_POINTS_REQUEST, purchasePointsSaga)
  yield takeEvery(CHAT_ACTION_TYPES.USE_POINTS_REQUEST, usePointsSaga)
  yield takeEvery(CHAT_ACTION_TYPES.SEND_MESSAGE_REQUEST, sendMessageSaga)
  
  // SignalR 連線只需要一個實例，使用 takeLatest
  yield takeLatest(CHAT_ACTION_TYPES.SIGNALR_CONNECT_REQUEST, signalRConnectionSaga)
}
