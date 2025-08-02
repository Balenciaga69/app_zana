import { Box, Flex, Text, IconButton, Button, useColorMode } from '@chakra-ui/react'
import { ArrowBackIcon } from '@chakra-ui/icons'
import { headerSx } from './ChatRoomHeader.style'
import { useNavigate } from 'react-router-dom'
import type { ReactNode } from 'react'

// 原子組件: 返回按鈕
const BackButton = ({ onClick, ariaLabel }: { onClick: () => void; ariaLabel: string }) => (
  <IconButton aria-label={ariaLabel} icon={<ArrowBackIcon />} sx={headerSx.backBtn} onClick={onClick} />
)

// 原子組件: 左側空白區塊
const LeftBox = () => {
  return <Box sx={headerSx.leftBox}></Box>
}

// 原子組件: 標題
const RoomTitle = ({ children }: { children: ReactNode }) => {
  return <Text sx={headerSx.title}>{children}</Text>
}

// 原子組件: 主題切換按鈕
const ThemeToggleButton = ({ toggleColorMode }: { toggleColorMode: () => void }) => {
  return (
    <Button sx={headerSx.themeBtn} onClick={toggleColorMode}>
      Dark
    </Button>
  )
}

// 原子組件: 右側彈性區塊
const RightFlex = ({ children }: { children: ReactNode }) => {
  return <Flex sx={headerSx.rightFlex}>{children}</Flex>
}

const ChatRoomHeader = () => {
  const navigate = useNavigate()
  const { toggleColorMode } = useColorMode()
  const sx = headerSx

  return (
    <Box sx={sx.container}>
      <Flex sx={sx.flex}>
        <BackButton onClick={() => navigate('/')} ariaLabel='離開房間' />
        <LeftBox />
        <RoomTitle>房間名稱</RoomTitle>
        <RightFlex>
          <ThemeToggleButton toggleColorMode={toggleColorMode} />
        </RightFlex>
      </Flex>
    </Box>
  )
}

export default ChatRoomHeader
