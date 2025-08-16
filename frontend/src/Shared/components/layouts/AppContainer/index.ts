export { default as AppContainer } from './AppContainer'

export type {
  AppContainerProps,
  AppContainerVariant,
  AppContainerStyles,
  AppContainerVariantStyles,
} from './AppContainer.types'

export { createAppContainerStyles, baseAppContainerStyles, variantStyles } from './AppContainer.style'

export const AppContainerVariants = {
  CHAT: 'chat',
  SETTINGS: 'settings',
  ROOM_LIST: 'roomList',
  PROFILE: 'profile',
  DEFAULT: 'default',
} as const
