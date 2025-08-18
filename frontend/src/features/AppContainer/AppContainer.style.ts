import type { AppContainerStyles } from './AppContainer.types'

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
 * 根據變體和選項生成最終樣式
 */
export const createAppContainerStyles = (
  options: {
    showShadow?: boolean
    fullscreen?: boolean
  } = {}
): AppContainerStyles => {
  const { showShadow = true, fullscreen = false } = options

  // 獲取變體樣式

  // 合併樣式
  const finalStyles: AppContainerStyles = {
    container: {
      ...baseAppContainerStyles.container,
      ...(fullscreen && {
        p: 0,
        minH: '100vh',
      }),
    },
    wrapper: {
      ...baseAppContainerStyles.wrapper,
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
    },
  }

  return finalStyles
}

/**
 * 匯出基礎樣式供進階使用
 */
export { baseAppContainerStyles }
