// Header 元件，包含專案標題與深色/淺色模式
import { Flex, Heading, Spacer, IconButton, useColorMode, Box, Text } from '@chakra-ui/react'
import { MoonIcon, SunIcon } from '@chakra-ui/icons'
import { headerStyles, headerTitleStyles, headerSubtitleStyles } from '../styles/componentStyles'

const Header = () => {
  const { colorMode, toggleColorMode } = useColorMode()
  return (
    <Flex sx={headerStyles(colorMode)}>
      <Box>
        <Heading sx={headerTitleStyles(colorMode)}>
          <span>Messages</span>
        </Heading>
        <Text sx={headerSubtitleStyles(colorMode)}>
          LGBTQ+Chat
        </Text>
      </Box>
      <Spacer />
      <IconButton
        aria-label='切換深色/淺色模式'
        icon={colorMode === 'light' ? <MoonIcon /> : <SunIcon />}
        onClick={toggleColorMode}
        variant='themeToggle'
      />
    </Flex>
  )
}

export default Header
