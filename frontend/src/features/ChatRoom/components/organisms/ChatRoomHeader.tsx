import {
  Box,
  Flex,
  Text,
  IconButton,
  Button,
  useColorMode,
  type BoxProps,
  type FlexProps,
  type TextProps,
  type ButtonProps,
  type IconButtonProps,
} from '@chakra-ui/react'
import { ArrowBackIcon } from '@chakra-ui/icons'
import { headerSx } from './ChatRoomHeader.style'
import { useNavigate } from 'react-router-dom'
import type { ReactNode } from 'react'

// 原子組件: 返回按鈕
interface BackButtonProps {
  onClick: () => void
  ariaLabel: string
  sx: Partial<IconButtonProps> & { ariaLabel: string }
}
const BackButton = ({ onClick, ariaLabel, sx }: BackButtonProps) => (
  <IconButton aria-label={ariaLabel} icon={<ArrowBackIcon />} sx={sx} onClick={onClick} />
)

// 原子組件: 左側空白區塊
interface LeftBoxProps {
  sx: BoxProps
}
const LeftBox = ({ sx }: LeftBoxProps) => <Box sx={sx}></Box>

// 原子組件: 標題
interface RoomTitleProps {
  sx: TextProps
  children: ReactNode
}
const RoomTitle = ({ sx, children }: RoomTitleProps) => <Text sx={sx}>{children}</Text>

// 原子組件: 主題切換按鈕
interface ThemeToggleButtonProps {
  sx: ButtonProps
  colorMode: string
  toggleColorMode: () => void
}
const ThemeToggleButton = ({ sx, colorMode, toggleColorMode }: ThemeToggleButtonProps) => (
  <Button sx={sx} onClick={toggleColorMode}>
    {colorMode === 'light' ? 'Dark' : 'Light'}
  </Button>
)

// 原子組件: 右側彈性區塊
interface RightFlexProps {
  sx: FlexProps
  children: ReactNode
}
const RightFlex = ({ sx, children }: RightFlexProps) => <Flex sx={sx}>{children}</Flex>

const ChatRoomHeader = () => {
  const navigate = useNavigate()
  const { colorMode, toggleColorMode } = useColorMode()
  const sx = headerSx(colorMode)

  return (
    <Box sx={sx.container}>
      <Flex sx={sx.flex}>
        <BackButton onClick={() => navigate('/')} ariaLabel={sx.backBtn.ariaLabel} sx={sx.backBtn} />
        <LeftBox sx={sx.leftBox} />
        <RoomTitle sx={sx.title}>房間名稱</RoomTitle>
        <RightFlex sx={sx.rightFlex}>
          <ThemeToggleButton sx={sx.themeBtn} colorMode={colorMode} toggleColorMode={toggleColorMode} />
        </RightFlex>
      </Flex>
    </Box>
  )
}

export default ChatRoomHeader
