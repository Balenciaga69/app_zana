import { Box, Flex, Text, IconButton, Button, useColorMode } from '@chakra-ui/react'
import { ArrowBackIcon } from '@chakra-ui/icons'
import { headerSx } from './ChatRoomHeader.style'

const ChatRoomHeader = () => {
  const { colorMode, toggleColorMode } = useColorMode()
  const sx = headerSx(colorMode)

  return (
    <Box sx={sx.container}>
      <Flex sx={sx.flex}>
          <IconButton aria-label={sx.backBtn.ariaLabel} icon={<ArrowBackIcon />} sx={sx.backBtn} />
        <Box sx={sx.leftBox}></Box>
        <Text sx={sx.title}>房間名稱</Text>
        <Flex sx={sx.rightFlex}>
          <Button sx={sx.themeBtn} onClick={toggleColorMode}>
            {colorMode === 'light' ? 'Dark' : 'Light'}
          </Button>
        </Flex>
      </Flex>
    </Box>
  )
}

export default ChatRoomHeader
