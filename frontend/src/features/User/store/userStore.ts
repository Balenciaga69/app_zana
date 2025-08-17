// filepath: c:\Custom\A-Code\my_SP\app_zana\frontend\src\features\User\store\userStore.ts
import { create } from 'zustand'
import { signalRService } from '@/Shared/SignalR/services/signalrService'
import { DeviceFingerprintHelper } from '@/Shared/SignalR/utils/deviceFingerprintHelper'

interface UserState {
  // 狀態
  userId: string | null
  nickname: string | null
  isNewUser: boolean
  connectionId: string | null
  serverTime: string | null
  registering: boolean
  updatingNickname: boolean
  error: string | null

  // 動作
  setRegistered: (payload: { userId: string; nickname: string | null; isNewUser: boolean }) => void
  setConnectionEstablished: (payload: { connectionId: string; serverTime: string }) => void
  setNickname: (nickname: string) => void
  setError: (error: string | null) => void
  resetError: () => void
  ensureRegistered: () => Promise<void>
  updateNickname: (newNickname: string) => Promise<void>
}

const initialState: Omit<
  UserState,
  | 'setRegistered'
  | 'setConnectionEstablished'
  | 'setNickname'
  | 'setError'
  | 'resetError'
  | 'ensureRegistered'
  | 'updateNickname'
> = {
  userId: null,
  nickname: null,
  isNewUser: false,
  connectionId: null,
  serverTime: null,
  registering: false,
  updatingNickname: false,
  error: null,
}

export const useUserStore = create<UserState>((set, get) => ({
  ...initialState,

  setRegistered: ({ userId, nickname, isNewUser }) => set({ userId, nickname, isNewUser }),
  setConnectionEstablished: ({ connectionId, serverTime }) => set({ connectionId, serverTime }),
  setNickname: (nickname) => set({ nickname }),
  setError: (error) => set({ error }),
  resetError: () => set({ error: null }),

  ensureRegistered: async () => {
    const state = get()
    if (state.registering || state.userId) return

    set({ registering: true, error: null })
    try {
      const fingerprint = await DeviceFingerprintHelper.getFingerprint()
      await signalRService.registerUser(fingerprint)
      // 成功後，實際的狀態更新由 SignalR 伺服器事件 UserRegistered/ConnectionEstablished 觸發
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Register failed'
      set({ error: errorMessage })
    } finally {
      set({ registering: false })
    }
  },

  updateNickname: async (newNickname: string) => {
    if (!newNickname?.trim()) return
    set({ updatingNickname: true, error: null })
    try {
      await signalRService.updateNickname(newNickname.trim())
      // 成功後由伺服器事件 NicknameUpdated 更新 nickname
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Update nickname failed'
      set({ error: errorMessage })
    } finally {
      set({ updatingNickname: false })
    }
  },
}))

export const userSelectors = {
  isRegistered: (s: UserState) => !!s.userId,
}
