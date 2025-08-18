import { type StateCreator, create } from 'zustand'

export interface UserState {
  nickname: string
  setNickname: (nickname: string) => void
}

export const createUserSlice: StateCreator<UserState> = (set) => ({
  // 狀態
  nickname: '',
  // 動作
  setNickname: (nickname) => set({ nickname }),
})

export const useUserStore = create<UserState>()(createUserSlice)
