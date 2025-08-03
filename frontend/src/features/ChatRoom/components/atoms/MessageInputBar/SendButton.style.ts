import type { SystemStyleObject } from '@chakra-ui/react'

export const sendButtonStyles: { button: SystemStyleObject } = {
  button: {
    padding: '8px 16px',
    fontSize: '1rem',
    bg: 'brand.primary',
    color: 'white',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
    _disabled: {
      opacity: 0.6,
      cursor: 'not-allowed',
    },
  },
}
