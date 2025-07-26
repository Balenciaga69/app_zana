// Chat 元件樣式配置 - 引用主題配置，避免重複定義
import type { SystemStyleObject, ThemingProps } from '@chakra-ui/react'
import theme from './theme'

// 從主題中取得顏色配置
const THEME_COLORS = theme.colors

// 從主題中取得間距配置
const THEME_SPACING = theme.spacing

// 從主題中取得字體大小配置
const THEME_FONT_SIZES = {
  ...theme.fontSizes,
} as const

// 從主題中取得圓角和陰影配置
const THEME_RADII = theme.radii
const THEME_SHADOWS = theme.shadows

// 工具函數：根據色彩模式選擇顏色
const getModeColor = (colorMode: string, darkColor: string, lightColor: string) =>
  colorMode === 'dark' ? darkColor : lightColor

// 工具函數：根據色彩模式選擇主題顏色
const getThemeColor = (colorMode: string, colorPath: 'bg' | 'card' | 'text') =>
  getModeColor(colorMode, THEME_COLORS[colorPath].dark, THEME_COLORS[colorPath].light)

// Header 樣式 - 引用主題配置
export const headerStyles = (colorMode: string): SystemStyleObject => ({
  align: 'center',
  mb: 6,
  ...THEME_SPACING.header,
  bg: getThemeColor(colorMode, 'card'),
  borderBottomWidth: 1,
  borderColor: getModeColor(colorMode, THEME_COLORS.gray[700], THEME_COLORS.gray[100]),
  borderRadius: THEME_RADII.message, // 使用主題中的圓角
  boxShadow: THEME_SHADOWS.subtle,   // 使用主題中的陰影
})

// Header 標題樣式 - 引用主題配置
export const headerTitleStyles = (colorMode: string): SystemStyleObject => ({
  fontWeight: 700,
  color: getModeColor(colorMode, THEME_COLORS.gray[100], THEME_COLORS.gray[800]),
  letterSpacing: 'wide',
  fontSize: THEME_FONT_SIZES.xlarge,
  mb: 0,
})

// Header 副標題樣式 - 引用主題配置
export const headerSubtitleStyles = (colorMode: string): SystemStyleObject => ({
  fontSize: THEME_FONT_SIZES.tiny,
  color: getModeColor(colorMode, THEME_COLORS.gray[500], THEME_COLORS.gray[400]),
  mt: -1,
  letterSpacing: 'wider',
  fontWeight: 400,
})

// 訊息輸入框樣式 - 引用主題配置
export const messageInputContainerStyles = (colorMode: string): SystemStyleObject => ({
  bg: getModeColor(colorMode, THEME_COLORS.gray[800], THEME_COLORS.gray[100]),
  borderRadius: THEME_RADII.message, // 使用主題中的圓角
  ...THEME_SPACING.input,
  boxShadow: THEME_SHADOWS.container, // 使用主題中的陰影
  gap: 2,
})

export const messageInputStyles = (colorMode: string): SystemStyleObject => ({
  variant: 'unstyled',
  fontSize: THEME_FONT_SIZES.normal,
  color: getModeColor(colorMode, THEME_COLORS.gray[100], THEME_COLORS.gray[800]),
  _placeholder: {
    color: getModeColor(colorMode, THEME_COLORS.gray[500], THEME_COLORS.gray[400]),
  },
})

// 訊息氣泡樣式 - 保留漸層但統一管理
const MESSAGE_GRADIENTS = {
  userDark: 'linear-gradient(135deg, #4F8CFF 60%, #6CA8FF 100%)',
  userLight: 'linear-gradient(135deg, #2563eb 60%, #60a5fa 100%)',
  otherDark: 'linear-gradient(135deg, #23243a 60%, #35365a 100%)',
  otherLight: 'linear-gradient(135deg, #f1f5f9 60%, #e0e7ef 100%)',
} as const

const MESSAGE_SHADOWS = {
  user: '0 6px 24px 0 rgba(80,140,255,0.18), 0 1.5px 6px 0 rgba(80,140,255,0.10)',
  other: '0 6px 24px 0 rgba(35,36,58,0.10), 0 1.5px 6px 0 rgba(35,36,58,0.06)',
} as const

export const messageBubbleStyles = (colorMode: string, isMe: boolean): SystemStyleObject => ({
  bg: isMe
    ? getModeColor(colorMode, MESSAGE_GRADIENTS.userDark, MESSAGE_GRADIENTS.userLight)
    : getModeColor(colorMode, MESSAGE_GRADIENTS.otherDark, MESSAGE_GRADIENTS.otherLight),
  color: isMe ? 'white' : getModeColor(colorMode, THEME_COLORS.gray[100], THEME_COLORS.gray[800]),
  ...THEME_SPACING.message,
  borderRadius: isMe ? '2.5em 2.5em 0.8em 2.5em' : '2.5em 2.5em 2.5em 0.8em',
  boxShadow: isMe ? MESSAGE_SHADOWS.user : MESSAGE_SHADOWS.other,
  position: 'relative',
  backdropFilter: 'blur(2px)',
})

// 用戶列表樣式 - 引用主題配置
export const userListStyles = (colorMode: string): SystemStyleObject => ({
  mb: 2,
  '& .userCount': {
    fontSize: THEME_FONT_SIZES.small,
    color: getModeColor(colorMode, THEME_COLORS.gray[300], THEME_COLORS.gray[500]),
  },
  '& .userTag': {
    fontSize: THEME_FONT_SIZES.tiny,
    bg: getModeColor(colorMode, THEME_COLORS.gray[700], THEME_COLORS.gray[200]),
    px: 2,
    borderRadius: THEME_RADII.card, // 使用主題中的圓角
  },
})

// 訊息頭像樣式 - 引用主題配置
export const messageAvatarStyles = (isMe: boolean): SystemStyleObject | ThemingProps<'Avatar'> => ({
  bg: isMe ? 'blue.400' : 'gray.400',
  color: 'white',
  boxShadow: THEME_SHADOWS.message, // 使用主題中的陰影
  ml: isMe ? 2 : 0,
  mr: !isMe ? 2 : 0,
})

// ChatRoom 外層 Flex 樣式 - 使用主題背景漸層
export const chatRoomContainerStyles: SystemStyleObject = {
  minH: '100vh',
  alignItems: 'center',
  justifyContent: 'center',
  bgGradient: 'linear(135deg, #23243a 0%, #3a3b5a 100%)',
  p: 4,
}

// ChatRoom 主要 Box 樣式 - 引用主題配置
export const chatRoomBoxStyles = (colorMode: string): SystemStyleObject => ({
  w: { base: '100%', sm: '390px' },
  maxW: '100vw',
  h: { base: '100vh', sm: '700px' },
  maxH: '100vh',
  bg: getModeColor(colorMode, 'rgba(30,32,48,0.95)', 'rgba(247,247,248,0.95)'),
  borderRadius: '2xl',
  boxShadow: '2xl',
  overflow: 'hidden',
  display: 'flex',
  flexDirection: 'column',
  position: 'relative',
  // Chakra 沒有 backdropFilter 內建，需用 style 傳遞
  style: { backdropFilter: 'blur(8px)' },
})

// ChatRoom 內層訊息區 Box 樣式
export const chatRoomInnerBoxStyles: SystemStyleObject = {
  flex: 1,
  display: 'flex',
  flexDirection: 'column',
  p: 0,
  overflowY: 'auto',
  // 隱藏 scrollbar，支援桌機與手機
  style: {
    scrollbarWidth: 'none', // Firefox
    msOverflowStyle: 'none', // IE/Edge
    '::-webkit-scrollbar': { display: 'none' }, // Chrome/Safari
    WebkitOverflowScrolling: 'touch', // iOS 無慣性滾動
  },
}

// 訊息列表區塊
export const chatRoomMessageListBoxStyles: SystemStyleObject = {
  flex: 1,
  overflowY: 'auto',
  mb: 2,
}

// 輸入區塊 - 引用主題間距
export const chatRoomInputBoxStyles: SystemStyleObject = {
  ...THEME_SPACING.input,
  pb: 3,
}
