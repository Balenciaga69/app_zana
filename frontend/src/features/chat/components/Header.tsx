// Header 元件，包含專案標題與深色/淺色模式，連線狀態與用戶資訊
import { Flex, Heading, Spacer, IconButton, useColorMode, Box, Text, HStack, Badge, Avatar } from '@chakra-ui/react'
import { MoonIcon, SunIcon } from '@chakra-ui/icons'
import { headerStyles, headerTitleStyles, headerSubtitleStyles } from '../styles/componentStyles'
import { useChatStore } from '../store/chatStore'

const Header = () => {
  const { colorMode, toggleColorMode } = useColorMode()
  const { currentUser, isConnected, connectionStatus } = useChatStore()

  return (
    <Flex sx={headerStyles(colorMode)}>
      <Box>
        <Heading sx={headerTitleStyles(colorMode)}>
          <span>Messages</span>
        </Heading>
        <Text sx={headerSubtitleStyles(colorMode)}>LGBTQ+Chat</Text>
      </Box>
      <Spacer />
      
      {/* 連線狀態與用戶資訊 */}
      <HStack spacing={3}>
        {currentUser && (
          <HStack spacing={2}>
            <Avatar size="sm" name={currentUser} />
            <Text fontSize="sm" fontWeight="medium">
              {currentUser}
            </Text>
          </HStack>
        )}
        
        <Badge 
          colorScheme={isConnected ? 'green' : 'red'} 
          variant="subtle"
          fontSize="xs"
        >
          {connectionStatus}
        </Badge>
        
        <IconButton
          aria-label='切換深色/淺色模式'
          icon={colorMode === 'light' ? <MoonIcon /> : <SunIcon />}
          onClick={toggleColorMode}
          variant='themeToggle'
        />      </HStack>
    </Flex>
  )
}

export default Header
