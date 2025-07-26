// 自訂 Chakra UI 主題，仿 ChatGPT 風格
import { extendTheme } from '@chakra-ui/react'
import type { ThemeConfig } from '@chakra-ui/react'

const config: ThemeConfig = {
  initialColorMode: 'light',
  useSystemColorMode: false,
}

// 通用工具函數：根據色彩模式選擇顏色
const colorMode = (darkColor: string, lightColor: string) => (props: any) =>
  props.colorMode === 'dark' ? darkColor : lightColor

// 常用的間距設定
const spacing = {
  message: { px: 6, py: 4 },
  input: { px: 3, py: 2 },
  header: { px: 4, py: 3 },
  compact: { px: 2, py: 1 },
}

// 常用的字體大小
const fontSizes = {
  tiny: 'xs',
  small: 'sm',
  normal: 'md',
  large: 'xl',
}

// 常用的圓角設定
const radii = {
  message: '2xl',
  button: 'full',
  card: 'md',
}

// 常用的陰影設定
const shadows = {
  message: '0 2px 8px 0 rgba(0,0,0,0.10)',
  container: 'md',
  subtle: 'sm',
}

// 常用的透明度設定
const opacities = {
  subtle: 0.5,
  medium: 0.7,
}

// 預設的文字樣式組合
const textStyles = {
  tiny: {
    fontSize: fontSizes.tiny,
  },
  tinySubtle: {
    fontSize: fontSizes.tiny,
    opacity: opacities.subtle,
  },
  tinyMedium: {
    fontSize: fontSizes.tiny,
    opacity: opacities.medium,
    fontWeight: 'semibold',
  },
  small: {
    fontSize: fontSizes.small,
  },
}

const colors = {
  // 參考 ChatGPT 網站配色
  brand: {
    50: '#e3f2fd',
    100: '#bbdefb',
    200: '#90caf9',
    300: '#64b5f6',
    400: '#42a5f5',
    500: '#2196f3',
    600: '#1e88e5',
    700: '#1976d2',
    800: '#1565c0',
    900: '#0d47a1',
  },
  // 亮色/暗色背景
  bg: {
    light: '#f7f7f8',
    dark: '#23272f',
  },
  // 主要文字
  text: {
    light: '#222222',
    dark: '#ececf1',
  },
  // 卡片背景
  card: {
    light: '#fff',
    dark: '#343541',
  },
  // 次要灰色
  gray: {
    50: '#f7fafc',
    100: '#e3e3e3',
    200: '#c8c8c8',
    300: '#a0a0a0',
    400: '#888888',
    500: '#666666',
    600: '#444444',
    700: '#333333',
    800: '#222222',
    900: '#111111',
  },
}

const theme = extendTheme({
  config,
  colors,
  // 將常用設定加入 theme 中
  spacing,
  fontSizes,
  radii,
  shadows,
  textStyles,
  styles: {
    global: (props: any) => ({
      body: {
        bg: colorMode('bg.dark', 'bg.light')(props),
        color: colorMode('text.dark', 'text.light')(props),
      },
    }),
  },
  components: {
    // Layout 相關元件樣式
    Box: {
      variants: {
        messageBubble: {
          ...spacing.message,
          position: 'relative',
          style: { backdropFilter: 'blur(2px)' },
        },
        messageContainer: {
          maxW: '75%',
          display: 'flex',
          flexDirection: 'column',
        },
      },
    },

    Flex: {
      variants: {
        header: (props: any) => ({
          align: 'center',
          mb: 6,
          ...spacing.header,
          bg: colorMode('card.dark', 'card.light')(props),
          borderBottomWidth: 1,
          borderColor: colorMode('gray.700', 'gray.100')(props),
          borderRadius: radii.message,
          boxShadow: shadows.subtle,
        }),
        messageRow: {
          direction: 'row',
          align: 'flex-end',
          mb: 3,
          ...spacing.compact,
        },
      },
    },

    // Stack 相關元件樣式
    VStack: {
      variants: {
        messageList: {
          align: 'stretch',
          spacing: 2,
          h: '100%',
          overflowY: 'auto',
          pb: 2,
        },
      },
    },

    HStack: {
      variants: {
        messageInput: (props: any) => ({
          bg: colorMode('gray.800', 'gray.100')(props),
          borderRadius: radii.message,
          ...spacing.input,
          boxShadow: shadows.container,
          spacing: 2,
        }),
      },
    },

    // 表單元件樣式
    Input: {
      variants: {
        messageInput: (props: any) => ({
          field: {
            variant: 'unstyled',
            fontSize: fontSizes.normal,
            color: colorMode('gray.100', 'gray.800')(props),
            _placeholder: {
              color: colorMode('gray.500', 'gray.400')(props),
            },
          },
        }),
      },
    },

    // 按鈕元件樣式
    IconButton: {
      variants: {
        messageAction: {
          borderRadius: radii.button,
          fontSize: fontSizes.large,
        },
        themeToggle: (props: any) => ({
          variant: 'ghost',
          size: 'lg',
          color: colorMode('gray.300', 'gray.600')(props),
          _hover: {
            bg: colorMode('gray.700', 'gray.200')(props),
          },
        }),
      },
    },

    // 文字元件樣式
    Text: {
      variants: {
        timestamp: {
          ...textStyles.tinySubtle,
          minW: '44px',
          textAlign: 'center',
        },
        userName: {
          ...textStyles.tinyMedium,
          mb: 1,
          px: 1,
        },
        userCount: (props: any) => ({
          fontSize: fontSizes.small,
          color: colorMode('gray.300', 'gray.500')(props),
        }),
        userTag: (props: any) => ({
          fontSize: fontSizes.tiny,
          bg: colorMode('gray.700', 'gray.200')(props),
          px: 2,
          borderRadius: radii.card,
        }),
      },
    },
  },
})

export default theme
