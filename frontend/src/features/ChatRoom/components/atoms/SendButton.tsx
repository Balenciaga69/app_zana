import { Button } from '@chakra-ui/react'
import { sendButtonStyles } from './SendButton.style'
import React from 'react'

interface SendButtonProps {
  onClick: () => void
  disabled?: boolean
  children?: React.ReactNode
}

const SendButton: React.FC<SendButtonProps> = ({ onClick, disabled, children }) => {
  return (
    <Button onClick={onClick} isDisabled={disabled} sx={sendButtonStyles.button} data-testid='SendButton'>
      {children ?? 'Send'}
    </Button>
  )
}

export default SendButton
