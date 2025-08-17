export { default as AppContainer } from './AppContainer'

export type { AppContainerProps, AppContainerStyles } from './AppContainer.types'

export { baseAppContainerStyles, createAppContainerStyles } from './AppContainer.style'

export const AppContainerVariants = {
  CHAT: 'chat',
  SETTINGS: 'settings',
  ROOM_LIST: 'roomList',
  PROFILE: 'profile',
  DEFAULT: 'default',
} as const
