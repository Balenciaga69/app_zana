// 範例組件：房間資訊顯示
import { Box, VStack, HStack, Text, Badge, Button, useToast } from '@chakra-ui/react'
import { useRoomStats, useUserManager, useChatPointsBalance } from '../../../hooks/useChatHooks'

interface RoomInfoProps {
  roomId: string
}

const RoomInfo = ({ roomId }: RoomInfoProps) => {
  const { data: roomStats, loading: roomLoading } = useRoomStats(roomId)
  const { data: pointsData, loading: pointsLoading } = useChatPointsBalance()
  const { updateNickname, purchasePoints } = useUserManager()
  const toast = useToast()

  const handleNicknameChange = async () => {
    try {
      await updateNickname.execute('新暱稱')
    } catch (error) {
      // 錯誤已在 hook 中處理
    }
  }

  const handlePurchasePoints = async () => {
    try {
      await purchasePoints.execute({
        amount: 100,
        paymentMethod: 'credit_card'
      })
    } catch (error) {
      // 錯誤已在 hook 中處理
    }
  }

  if (roomLoading || pointsLoading) {
    return <Text>載入中...</Text>
  }

  return (
    <VStack spacing={4} p={4} border="1px" borderColor="gray.200" borderRadius="md">
      <Text fontSize="lg" fontWeight="bold">房間資訊</Text>
      
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

      {pointsData && (
        <HStack>
          <Text>聊天點數:</Text>
          <Badge colorScheme="blue">{pointsData.balance}</Badge>
        </HStack>
      )}

      <VStack spacing={2}>
        <Button 
          onClick={handleNicknameChange}
          isLoading={updateNickname.loading}
          size="sm"
        >
          更改暱稱
        </Button>
        
        <Button 
          onClick={handlePurchasePoints}
          isLoading={purchasePoints.loading}
          colorScheme="green"
          size="sm"
        >
          購買點數
        </Button>
      </VStack>
    </VStack>
  )
}

export default RoomInfo
