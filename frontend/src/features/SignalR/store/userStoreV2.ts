import { create } from 'zustand'

export interface UserStateV2 {
  nickname: string
  setNickname: (nickname: string) => void
}

export const useUserStoreV2 = create<UserStateV2>((set) => ({
  nickname: '',
  setNickname: (nickname) => set({ nickname }),
}))
