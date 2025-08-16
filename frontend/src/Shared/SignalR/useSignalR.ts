import { useSignalRStore, signalRSelectors } from './signalrStore'

export const useSignalR = () => {
  const { connectionState, connection, error, isConnecting, connect, disconnect } = useSignalRStore()

  return {
    // 狀態
    connectionState,
    connection,
    error,
    isConnecting,

    // 計算屬性
    isConnected: signalRSelectors.isConnected(useSignalRStore.getState()),
    isDisconnected: signalRSelectors.isDisconnected(useSignalRStore.getState()),
    hasError: signalRSelectors.hasError(useSignalRStore.getState()),
    canSendMessage: signalRSelectors.canSendMessage(useSignalRStore.getState()),

    // 動作
    connect,
    disconnect,
  }
}

// TODO: 後續可以添加更多專用的 hooks
// export const useChatRoom = () => { ... }
// export const useRoomManagement = () => { ... }
