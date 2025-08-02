import type { SystemStyleObject, FlexProps } from '@chakra-ui/react'

interface ChatWindowStyles {
  container: SystemStyleObject & FlexProps
  wrapper: SystemStyleObject & FlexProps
}

export const chatWindowStyles: ChatWindowStyles = {
  container: {
    alignItems: 'center',
    justifyContent: 'center',
    bg: 'ui.background',
    _dark: { bg: 'ui.background' },
    minH: '100vh',
    p: { base: 0, md: 4 },
  },

  wrapper: {
    as: 'main',
    flexDirection: 'column',
    w: '100%',
    h: { base: '100vh', md: '90vh' },
    maxW: { base: '100%', md: '800px' },
    maxH: { base: '100vh', md: '1000px' },
    bg: 'ui.background',
    borderWidth: { base: 0, md: '1px' },
    borderColor: 'ui.border',
    borderRadius: { base: 'none', md: 'lg' },
    boxShadow: { base: 'none', md: 'xl' },
    overflow: 'hidden',
  },
}
