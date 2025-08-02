import { Text, type TextProps } from '@chakra-ui/react'
import { timestampStyles } from './Timestamp.style'

export interface TimestampProps extends TextProps {
  time: string
}

export const Timestamp = ({ time, ...rest }: TimestampProps) => (
  <Text sx={timestampStyles.text} data-testid='Timestamp' {...rest}>
    {time}
  </Text>
)

export default Timestamp
