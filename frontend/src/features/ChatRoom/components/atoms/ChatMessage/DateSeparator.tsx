import { Flex, Text } from '@chakra-ui/react'
import type { FC } from 'react'
import { dateSeparatorStyles } from './DateSeparator.style'

interface DateSeparatorProps {
  date: string // 建議格式: YYYY-MM-DD 或已格式化
}

const DateSeparator: FC<DateSeparatorProps> = ({ date }) => {
  return (
    <Flex sx={dateSeparatorStyles.container} data-testid='date-separator'>
      <Flex sx={dateSeparatorStyles.line} />
      <Text sx={dateSeparatorStyles.text}>{date}</Text>
      <Flex sx={dateSeparatorStyles.line} />
    </Flex>
  )
}

export default DateSeparator
