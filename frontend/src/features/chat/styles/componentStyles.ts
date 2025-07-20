// Chat 元件樣式配置
import type { SystemStyleObject } from '@chakra-ui/react'

// Header 樣式
export const headerStyles = (colorMode: string): SystemStyleObject => ({
  align: 'center',
  mb: 6,
  px: 4,
  py: 3,
  bg: colorMode === 'dark' ? 'card.dark' : 'card.light',
  borderBottomWidth: 1,
  borderColor: colorMode === 'dark' ? 'gray.700' : 'gray.100',
  borderRadius: '2xl',
  boxShadow: 'sm',
})

// Header 標題樣式
export const headerTitleStyles = (colorMode: string): SystemStyleObject => ({
  fontWeight: 700,
  color: colorMode === 'dark' ? 'gray.100' : 'gray.800',
  letterSpacing: 'wide',
  fontSize: '2xl',
  mb: 0,
})

// Header 副標題樣式
export const headerSubtitleStyles = (colorMode: string): SystemStyleObject => ({
  fontSize: 'xs',
  color: colorMode === 'dark' ? 'gray.500' : 'gray.400',
  mt: -1,
  letterSpacing: 'wider',
  fontWeight: 400,
})

// 訊息輸入框樣式
export const messageInputContainerStyles = (colorMode: string): SystemStyleObject => ({
  bg: colorMode === 'dark' ? 'gray.800' : 'gray.100',
  borderRadius: '2xl',
  px: 3,
  py: 2,
  boxShadow: 'md',
  gap: 2,
})

export const messageInputStyles = (colorMode: string): SystemStyleObject => ({
  variant: 'unstyled',
  fontSize: 'md',
  color: colorMode === 'dark' ? 'gray.100' : 'gray.800',
  _placeholder: {
    color: colorMode === 'dark' ? 'gray.500' : 'gray.400',
  },
})

// 訊息氣泡樣式
export const messageBubbleStyles = (colorMode: string, isMe: boolean): SystemStyleObject => ({
  bg: isMe
    ? colorMode === 'dark'
      ? 'linear-gradient(135deg, #4F8CFF 60%, #6CA8FF 100%)'
      : 'linear-gradient(135deg, #2563eb 60%, #60a5fa 100%)'
    : colorMode === 'dark'
      ? 'linear-gradient(135deg, #23243a 60%, #35365a 100%)'
      : 'linear-gradient(135deg, #f1f5f9 60%, #e0e7ef 100%)',
  color: isMe ? 'white' : colorMode === 'dark' ? 'gray.100' : 'gray.800',
  px: 6,
  py: 4,
  borderRadius: isMe ? '2.5em 2.5em 0.8em 2.5em' : '2.5em 2.5em 2.5em 0.8em',
  boxShadow: isMe
    ? '0 6px 24px 0 rgba(80,140,255,0.18), 0 1.5px 6px 0 rgba(80,140,255,0.10)'
    : '0 6px 24px 0 rgba(35,36,58,0.10), 0 1.5px 6px 0 rgba(35,36,58,0.06)',
  position: 'relative',
  backdropFilter: 'blur(2px)',
})

// 用戶列表樣式
export const userListStyles = (colorMode: string): SystemStyleObject => ({
  mb: 2,
  '& .userCount': {
    fontSize: 'sm',
    color: colorMode === 'dark' ? 'gray.300' : 'gray.500',
  },
  '& .userTag': {
    fontSize: 'xs',
    bg: colorMode === 'dark' ? 'gray.700' : 'gray.200',
    px: 2,
    borderRadius: 'md',
  },
})

// ChatRoom 外層 Flex 樣式
export const chatRoomContainerStyles: SystemStyleObject = {
  minH: '100vh',
  alignItems: 'center',
  justifyContent: 'center',
  bgGradient: 'linear(135deg, #23243a 0%, #3a3b5a 100%)',
  p: 4,
}

// ChatRoom 主要 Box 樣式
export const chatRoomBoxStyles = (colorMode: string): SystemStyleObject => ({
  w: { base: '100%', sm: '390px' },
  maxW: '100vw',
  h: { base: '100vh', sm: '700px' },
  maxH: '100vh',
  bg: colorMode === 'dark' ? 'rgba(30,32,48,0.95)' : 'rgba(247,247,248,0.95)',
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

// 輸入區塊
export const chatRoomInputBoxStyles: SystemStyleObject = {
  px: 3,
  pb: 3,
}
