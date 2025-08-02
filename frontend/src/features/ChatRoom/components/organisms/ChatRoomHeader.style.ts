import type { BoxProps, FlexProps, TextProps, ButtonProps, IconButtonProps } from '@chakra-ui/react'

export interface HeaderSx {
  container: BoxProps
  flex: FlexProps
  leftBox: BoxProps
  title: TextProps
  rightFlex: FlexProps
  themeBtn: ButtonProps
  backBtn: Partial<IconButtonProps>
}

export const headerSx: HeaderSx = {
  container: {
    w: '100%',
    boxShadow: 'md',
    px: 4,
    py: 2,
    position: 'sticky',
    top: 0,
    zIndex: 10,
    borderRadius: 'md',
    borderBottom: '1px solid',
    bg: 'ui.background',
    borderColor: 'ui.border',
  },
  flex: {
    w: '100%',
    minH: '56px',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  leftBox: {
    minW: '32px',
  },
  title: {
    flex: 1,
    textAlign: 'left',
    fontWeight: 'bold',
    fontSize: 'xl',
    color: 'text.primary',
    letterSpacing: '0.5px',
  },
  rightFlex: {
    align: 'center',
    gap: 2,
  },
  themeBtn: {
    size: 'sm',
    bg: 'brand.primary',
    color: 'white',
    fontWeight: 'bold',
    borderRadius: 'md',
    px: 3,
  },
  backBtn: {
    minW: '32px',
    fontSize: 'xl',
    borderRadius: 'md',
    bg: 'transparent',
    color: 'text.primary',
  },
}
