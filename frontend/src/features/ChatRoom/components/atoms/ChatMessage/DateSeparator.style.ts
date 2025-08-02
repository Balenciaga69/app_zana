import type { SystemStyleObject } from '@chakra-ui/react'

export const dateSeparatorStyles: { container: SystemStyleObject; line: SystemStyleObject; text: SystemStyleObject } = {
  container: {
    alignItems: 'center',
    my: 4,
    display: 'flex',
  },
  line: {
    flex: 1,
    height: '1px',
    bg: 'ui.border',
  },
  text: {
    px: 3,
    fontSize: 'sm',
    color: 'text.secondary',
    fontWeight: 'medium',
    whiteSpace: 'nowrap',
  },
}
