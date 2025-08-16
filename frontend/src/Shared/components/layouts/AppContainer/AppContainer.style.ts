import type { AppContainerStyles, AppContainerVariantStyles } from './AppContainer.types'

/**
 * 基礎 AppContainer 樣式
 * 繼承自原始的 chatWindowStyles，作為所有變體的基礎
 */
const baseAppContainerStyles: AppContainerStyles = {
  container: {
    alignItems: 'center',
    justifyContent: 'center',
    bg: 'ui.background',
    _dark: { bg: 'ui.background' },
    minH: '100vh',
    p: { base: 0, md: 4 },
  },

  wrapper: {
    as: 'main',
    flexDirection: 'column',
    w: '100%',
    h: { base: '100vh', md: '90vh' },
    maxW: { base: '100%', md: '800px' },
    maxH: { base: '100vh', md: '1000px' },
    bg: 'ui.background',
    borderWidth: { base: 0, md: '1px' },
    borderColor: 'ui.border',
    borderRadius: { base: 'none', md: 'lg' },
    boxShadow: { base: 'none', md: 'xl' },
    overflow: 'hidden',
  },
}

/**
 * 各變體的樣式覆蓋配置
 */
const variantStyles: AppContainerVariantStyles = {
  // 聊天室變體（完全繼承基礎樣式，即原 chatWindowStyles）
  chat: {},

  // 設定頁面變體
  settings: {
    wrapper: {
      maxW: { base: '100%', md: '600px' },
      h: { base: '100vh', md: 'auto' },
      maxH: { base: '100vh', md: '800px' },
      minH: { base: '100vh', md: '500px' },
    },
  },

  // 房間列表變體
  roomList: {
    wrapper: {
      maxW: { base: '100%', md: '900px' },
      h: { base: '100vh', md: 'auto' },
      maxH: { base: '100vh', md: '900px' },
      minH: { base: '100vh', md: '600px' },
    },
  },

  // 個人資料變體
  profile: {
    wrapper: {
      maxW: { base: '100%', md: '500px' },
      h: { base: '100vh', md: 'auto' },
      maxH: { base: '100vh', md: '700px' },
      minH: { base: '100vh', md: '400px' },
    },
  },

  // 預設變體（較小的通用容器）
  default: {
    wrapper: {
      maxW: { base: '100%', md: '600px' },
      h: { base: '100vh', md: 'auto' },
      maxH: { base: '100vh', md: '800px' },
      minH: { base: '100vh', md: '300px' },
    },
  },
}

/**
 * 根據變體和選項生成最終樣式
 */
export const createAppContainerStyles = (
  variant: keyof AppContainerVariantStyles = 'default',
  options: {
    showShadow?: boolean
    fullscreen?: boolean
    containerSx?: any
    wrapperSx?: any
  } = {}
): AppContainerStyles => {
  const { showShadow = true, fullscreen = false, containerSx = {}, wrapperSx = {} } = options

  // 獲取變體樣式
  const variantOverrides = variantStyles[variant] || {}

  // 合併樣式
  const finalStyles: AppContainerStyles = {
    container: {
      ...baseAppContainerStyles.container,
      ...variantOverrides.container,
      ...(fullscreen && {
        p: 0,
        minH: '100vh',
      }),
      ...containerSx,
    },
    wrapper: {
      ...baseAppContainerStyles.wrapper,
      ...variantOverrides.wrapper,
      ...(fullscreen && {
        h: '100vh',
        maxH: '100vh',
        borderWidth: 0,
        borderRadius: 'none',
        boxShadow: 'none',
      }),
      ...(!showShadow && {
        boxShadow: 'none',
      }),
      ...wrapperSx,
    },
  }

  return finalStyles
}

/**
 * 匯出基礎樣式供進階使用
 */
export { baseAppContainerStyles, variantStyles }
