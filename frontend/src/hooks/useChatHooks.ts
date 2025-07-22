// 聊天室功能的專用 Hooks
import { useCallback } from 'react'
import { useApi, useApiEffect } from './useApi'
import { chatApi, type RoomStats, type UserProfile, type PurchasePointsRequest } from '../services/chatApi'
import { useChatStore } from '../features/chat/store/chatStore'

// 房間統計 Hook
export function useRoomStats(roomId: string) {
  return useApiEffect(chatApi.getRoomStats, { roomId }, [roomId])
}

// 使用者資料 Hook
export function useUserProfile(userId?: string) {
  return useApiEffect(chatApi.getUserProfile, { userId }, [userId])
}

// 更新暱稱 Hook
export function useUpdateNickname() {
  const { setCurrentUser } = useChatStore()
  
  return useApi(chatApi.updateNickname, {
    showToast: true,
    onSuccess: (profile) => {
      // 更新 Zustand store 中的使用者名稱
      setCurrentUser(profile.nickname)
    },
  })
}

// 聊天點數餘額 Hook
export function useChatPointsBalance() {
  return useApiEffect(chatApi.getChatPointsBalance, {}, [])
}

// 購買聊天點數 Hook
export function usePurchaseChatPoints() {
  return useApi(chatApi.purchaseChatPoints, {
    showToast: true,
    onSuccess: (response) => {
      console.log('購買成功，新餘額:', response.newBalance)
    },
  })
}

// 使用聊天點數 Hook
export function useUseChatPoints() {
  return useApi(chatApi.useChatPoints, {
    onSuccess: (response) => {
      console.log('使用點數成功，剩餘:', response.newBalance)
    },
  })
}

// 複合 Hook：完整的使用者管理
export function useUserManager() {
  const updateNickname = useUpdateNickname()
  const purchasePoints = usePurchaseChatPoints()
  const usePoints = useUseChatPoints()
  
  const handleNicknameChange = useCallback(async (nickname: string) => {
    if (!nickname.trim()) {
      throw new Error('暱稱不能為空')
    }
    return await updateNickname.execute({ nickname })
  }, [updateNickname])

  const handlePurchasePoints = useCallback(async (request: PurchasePointsRequest) => {
    return await purchasePoints.execute(request)
  }, [purchasePoints])

  const handleUsePoints = useCallback(async (points: number, action: string) => {
    return await usePoints.execute({ points, action })
  }, [usePoints])

  return {
    updateNickname: {
      ...updateNickname,
      execute: handleNicknameChange,
    },
    purchasePoints: {
      ...purchasePoints,
      execute: handlePurchasePoints,
    },
    usePoints: {
      ...usePoints,
      execute: handleUsePoints,
    },
  }
}
