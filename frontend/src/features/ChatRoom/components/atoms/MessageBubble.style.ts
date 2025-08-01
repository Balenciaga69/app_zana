import type { SystemStyleObject } from '@chakra-ui/react'

export const messageBubbleStyles: Record<'own' | 'other', SystemStyleObject> = {
  own: {
    bg: 'chat.ownBubbleBg',
    px: 4,
    py: 2,
    borderRadius: 'lg',
    maxW: '70%',
    alignSelf: 'flex-end',
    color: 'text.primary',
  },
  other: {
    bg: 'chat.otherBubbleBg',
    px: 4,
    py: 2,
    borderRadius: 'lg',
    maxW: '70%',
    alignSelf: 'flex-start',
    color: 'text.primary',
  },
}
