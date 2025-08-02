import { extendTheme, type ThemeConfig } from '@chakra-ui/react'

const config: ThemeConfig = {
  initialColorMode: 'light',
  useSystemColorMode: false,
}

const standardColors = {
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
const githubColors = {
  brand: {
    primary: '#24292f', // GitHub主色
    secondary: '#0366d6', // GitHub藍
  },
  ui: {
    background: {
      light: '#f6f8fa',
      dark: '#0d1117',
    },
    border: {
      light: '#d0d7de',
      dark: '#30363d',
    },
  },
  text: {
    primary: {
      light: '#24292f',
      dark: '#c9d1d9',
    },
    secondary: {
      light: '#57606a',
      dark: '#8b949e',
    },
  },
  chat: {
    ownBubbleBg: {
      light: '#d2eafd', // 較淡藍
      dark: '#161b22',
    },
    otherBubbleBg: {
      light: '#eaeef2', // 較淡灰
      dark: '#21262d',
    },
  },
}
const fonts = {
  heading: `'Inter', sans-serif`,
  body: `'Inter', sans-serif`,
}
const currColors = githubColors
const theme = extendTheme({
  config,
  semanticTokens: {
    colors: {
      'brand.primary': { default: currColors.brand.primary },
      'brand.secondary': { default: currColors.brand.secondary },
      'ui.background': { default: currColors.ui.background.light, _dark: currColors.ui.background.dark },
      'ui.border': { default: currColors.ui.border.light, _dark: currColors.ui.border.dark },
      'text.primary': { default: currColors.text.primary.light, _dark: currColors.text.primary.dark },
      'text.secondary': { default: currColors.text.secondary.light, _dark: currColors.text.secondary.dark },
      'chat.ownBubbleBg': { default: currColors.chat.ownBubbleBg.light, _dark: currColors.chat.ownBubbleBg.dark },
      'chat.otherBubbleBg': {
        default: currColors.chat.otherBubbleBg.light,
        _dark: currColors.chat.otherBubbleBg.dark,
      },
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
