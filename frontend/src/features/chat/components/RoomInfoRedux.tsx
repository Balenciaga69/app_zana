// Redux 年代的組件使用方式
import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { Box, VStack, HStack, Text, Badge, Button, useToast } from '@chakra-ui/react'
import * as actions from '../redux-example/actions'

interface RoomInfoReduxProps {
  roomId: string
}

const RoomInfoRedux = ({ roomId }: RoomInfoReduxProps) => {
  const dispatch = useDispatch()
  const toast = useToast()

  // 從 Redux store 選取需要的狀態
  const {
    roomStats,
    roomStatsLoading,
    roomStatsError,
    chatPoints,
    pointsLoading,
    currentUser,
    isConnected,
    connectionStatus,
    loading,
    toast: reduxToast
  } = useSelector((state: any) => state.chat)

  // 組件掛載時載入房間統計
  useEffect(() => {
    dispatch(actions.fetchRoomStatsRequest(roomId))
    dispatch({ type: 'FETCH_POINTS_BALANCE_REQUEST' })
  }, [dispatch, roomId])

  // 處理 Redux Toast
  useEffect(() => {
    if (reduxToast?.visible) {
      toast({
        title: reduxToast.message,
        status: reduxToast.type,
        duration: 3000,
        isClosable: true,
      })
      dispatch(actions.hideToast())
    }
  }, [reduxToast, toast, dispatch])

  // 事件處理器
  const handleNicknameChange = () => {
    dispatch(actions.updateNicknameRequest('新暱稱'))
  }

  const handlePurchasePoints = () => {
    dispatch(actions.purchasePointsRequest(100, 'credit_card'))
  }

  const handleUsePoints = () => {
    dispatch(actions.usePointsRequest(10, 'send_special_message'))
  }

  const handleConnect = () => {
    dispatch(actions.signalrConnectRequest())
  }

  if (roomStatsLoading || pointsLoading) {
    return <Text>載入中...</Text>
  }

  if (roomStatsError) {
    return <Text color="red.500">載入失敗: {roomStatsError}</Text>
  }

  return (
    <VStack spacing={4} p={4} border="1px" borderColor="gray.200" borderRadius="md">
      <Text fontSize="lg" fontWeight="bold">房間資訊 (Redux)</Text>
      
      {roomStats && (
        <VStack spacing={2}>
          <HStack>
            <Text>線上人數:</Text>
            <Badge colorScheme="green">{roomStats.userCount}</Badge>
          </HStack>
          <HStack>
            <Text>訊息數量:</Text>
            <Text>{roomStats.messageCount}</Text>
          </HStack>
        </VStack>
      )}

      <HStack>
        <Text>聊天點數:</Text>
        <Badge colorScheme="blue">{chatPoints}</Badge>
      </HStack>

      <HStack>
        <Text>連線狀態:</Text>
        <Badge colorScheme={isConnected ? 'green' : 'red'}>
          {connectionStatus}
        </Badge>
      </HStack>

      {currentUser && (
        <HStack>
          <Text>目前使用者:</Text>
          <Text fontWeight="bold">{currentUser}</Text>
        </HStack>
      )}

      <VStack spacing={2}>
        <Button 
          onClick={handleNicknameChange}
          isLoading={loading.nickname}
          size="sm"
        >
          更改暱稱
        </Button>
        
        <Button 
          onClick={handlePurchasePoints}
          isLoading={loading.purchase}
          colorScheme="green"
          size="sm"
        >
          購買點數 (100)
        </Button>

        <Button 
          onClick={handleUsePoints}
          isLoading={loading.usePoints}
          colorScheme="orange"
          size="sm"
        >
          使用點數 (10)
        </Button>

        <Button 
          onClick={handleConnect}
          isLoading={loading.nickname}
          colorScheme="blue"
          size="sm"
          isDisabled={isConnected}
        >
          {isConnected ? '已連線' : '連線 SignalR'}
        </Button>
      </VStack>
    </VStack>
  )
}

export default RoomInfoRedux
