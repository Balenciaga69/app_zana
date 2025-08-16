import { Flex } from '@chakra-ui/react'
import type { FC } from 'react'
import { createAppContainerStyles } from './AppContainer.style'
import type { AppContainerProps } from './AppContainer.types'

/**
 * AppContainer - 通用應用程式容器元件
 *
 * 提供統一的頁面佈局框架，支援多種變體和自訂選項。
 * 基於原始的 ChatWindow 設計，但更加靈活和可重用。
 *
 * @example
 * // 聊天室佈局
 * <AppContainer variant="chat">
 *   <ChatContent />
 * </AppContainer>
 *
 * @example
 * // 設定頁面佈局
 * <AppContainer variant="settings" showShadow={false}>
 *   <SettingsForm />
 * </AppContainer>
 *
 * @example
 * // 自訂樣式
 * <AppContainer
 *   variant="default"
 *   wrapperSx={{ bg: 'blue.50' }}
 *   fullscreen
 * >
 *   <CustomContent />
 * </AppContainer>
 */
const AppContainer: FC<AppContainerProps> = ({
  children,
  variant = 'default',
  containerSx = {},
  wrapperSx = {},
  testId = 'app-container',
  showShadow = true,
  fullscreen = false,
}) => {
  // 生成樣式
  const styles = createAppContainerStyles(variant, {
    showShadow,
    fullscreen,
    containerSx,
    wrapperSx,
  })

  return (
    <Flex sx={styles.container} data-testid={`${testId}-container`}>
      <Flex sx={styles.wrapper} data-testid={`${testId}-wrapper`}>
        {children}
      </Flex>
    </Flex>
  )
}

export default AppContainer
