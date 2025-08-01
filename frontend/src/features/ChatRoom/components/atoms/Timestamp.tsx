import { Text, type TextProps } from '@chakra-ui/react'

export interface TimestampProps extends TextProps {
  time: string
}

export const Timestamp = ({ time, ...rest }: TimestampProps) => (
  <Text fontSize='xs' color='text.secondary' data-testid='Timestamp' {...rest}>
    {time}
  </Text>
)

export default Timestamp
