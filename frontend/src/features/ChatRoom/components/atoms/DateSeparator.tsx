import { Flex, Text } from '@chakra-ui/react'
import type { FC } from 'react'
import { dateSeparatorStyles } from './DateSeparator.style'

interface DateSeparatorProps {
  date: string // 建議格式: YYYY-MM-DD 或已格式化
}

/**
 * DateSeparator
 * 聊天訊息流中的日期分隔元件，顯示日期並以線條區隔。
 * 僅負責視覺分隔，不處理訊息邏輯。
 */
const DateSeparator: FC<DateSeparatorProps> = ({ date }) => {
  return (
    <Flex sx={dateSeparatorStyles.container}>
      <Flex sx={dateSeparatorStyles.line} />
      <Text sx={dateSeparatorStyles.text}>{date}</Text>
      <Flex sx={dateSeparatorStyles.line} />
    </Flex>
  )
}

export default DateSeparator
