import { extendTheme } from '@chakra-ui/react'
import type { ThemeConfig } from '@chakra-ui/react'

const config: ThemeConfig = {
  initialColorMode: 'light',
  useSystemColorMode: false,
}

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
  components: {},
})

export default theme
