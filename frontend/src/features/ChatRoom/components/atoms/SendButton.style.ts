import type { SystemStyleObject } from '@chakra-ui/react'

export const sendButtonStyles: { button: SystemStyleObject } = {
  button: {
    padding: '8px 16px',
    fontSize: '1rem',
    background: '#1976d2',
    color: '#fff',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
    _disabled: {
      opacity: 0.6,
      cursor: 'not-allowed',
    },
  },
}
