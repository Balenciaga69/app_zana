import type { SystemStyleObject, FlexProps } from '@chakra-ui/react'
import type { ReactNode } from 'react'

/**
 * AppContainer 佈局變體類型
 */
export type AppContainerVariant =
  | 'chat' // 聊天室佈局（原 ChatWindow）
  | 'settings' // 設定頁面佈局
  | 'roomList' // 房間列表佈局
  | 'profile' // 個人資料佈局
  | 'default' // 預設佈局

/**
 * AppContainer 樣式定義介面
 */
export interface AppContainerStyles {
  container: SystemStyleObject & FlexProps
  wrapper: SystemStyleObject & FlexProps
}

/**
 * AppContainer 元件 Props
 */
export interface AppContainerProps {
  /** 子元件內容 */
  children: ReactNode

  /** 佈局變體類型 */
  variant?: AppContainerVariant

  /** 自訂樣式覆蓋 */
  containerSx?: SystemStyleObject
  wrapperSx?: SystemStyleObject

  /** 測試 ID */
  testId?: string

  /** 是否顯示陰影 */
  showShadow?: boolean

  /** 是否全螢幕模式 */
  fullscreen?: boolean
}

/**
 * 變體樣式配置映射
 */
export type AppContainerVariantStyles = Record<AppContainerVariant, Partial<AppContainerStyles>>
