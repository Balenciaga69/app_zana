import { extendTheme, type ThemeConfig } from '@chakra-ui/react'

const config: ThemeConfig = {
  initialColorMode: 'light',
  useSystemColorMode: false,
}

const colors = {
  brand: {
    primary: '#319795',
    secondary: '#4A5568',
  },

  ui: {
    background: {
      light: '#FFFFFF',
      dark: '#1A202C',
    },
    border: {
      light: '#E2E8F0',
      dark: '#2D3748',
    },
  },

  text: {
    primary: {
      light: '#2D3748',
      dark: '#E2E8F0',
    },
    secondary: {
      light: '#A0AEC0',
      dark: '#718096',
    },
  },

  chat: {
    ownBubbleBg: {
      light: '#B2F5EA',
      dark: '#2C7A7B',
    },
    otherBubbleBg: {
      light: '#EDF2F7',
      dark: '#2D3748',
    },
  },
}

const fonts = {
  heading: `'Inter', sans-serif`,
  body: `'Inter', sans-serif`,
}

const theme = extendTheme({
  config,
  semanticTokens: {
    colors: {
      'brand.primary': { default: colors.brand.primary },
      'brand.secondary': { default: colors.brand.secondary },
      'ui.background': { default: colors.ui.background.light, _dark: colors.ui.background.dark },
      'ui.border': { default: colors.ui.border.light, _dark: colors.ui.border.dark },
      'text.primary': { default: colors.text.primary.light, _dark: colors.text.primary.dark },
      'text.secondary': { default: colors.text.secondary.light, _dark: colors.text.secondary.dark },
      'chat.ownBubbleBg': { default: colors.chat.ownBubbleBg.light, _dark: colors.chat.ownBubbleBg.dark },
      'chat.otherBubbleBg': { default: colors.chat.otherBubbleBg.light, _dark: colors.chat.otherBubbleBg.dark },
    },
  },
  fonts,
  components: {
    Button: {
      baseStyle: {
        fontWeight: 'bold',
      },
      variants: {
        solid: (props: { colorMode: string }) => ({
          bg: props.colorMode === 'dark' ? 'brand.primary' : 'brand.primary',
          color: 'white',
        }),
      },
    },
  },
})

export default theme
