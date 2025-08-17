import type { FlexProps, SystemStyleObject } from '@chakra-ui/react'
import type { ReactNode } from 'react'

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
