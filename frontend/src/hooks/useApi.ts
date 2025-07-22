// 自定義 Hook 來處理 API 呼叫和狀態管理
import { useState, useEffect, useCallback } from 'react'
import { useToast } from '@chakra-ui/react'

// 通用的 API Hook
export function useApi<TData, TParams = void>(
  apiCall: (params: TParams) => Promise<TData>,
  options: {
    immediate?: boolean
    onSuccess?: (data: TData) => void
    onError?: (error: Error) => void
    showToast?: boolean
  } = {}
) {
  const [data, setData] = useState<TData | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<Error | null>(null)
  const toast = useToast()

  const execute = useCallback(
    async (params: TParams) => {
      try {
        setLoading(true)
        setError(null)
        const result = await apiCall(params)
        setData(result)

        if (options.onSuccess) {
          options.onSuccess(result)
        }

        if (options.showToast) {
          toast({
            title: '操作成功',
            status: 'success',
            duration: 3000,
            isClosable: true,
          })
        }

        return result
      } catch (err) {
        const error = err instanceof Error ? err : new Error('發生未知錯誤')
        setError(error)

        if (options.onError) {
          options.onError(error)
        }

        if (options.showToast) {
          toast({
            title: '操作失敗',
            description: error.message,
            status: 'error',
            duration: 5000,
            isClosable: true,
          })
        }

        throw error
      } finally {
        setLoading(false)
      }
    },
    [apiCall, options, toast]
  )

  const reset = useCallback(() => {
    setData(null)
    setError(null)
    setLoading(false)
  }, [])

  return {
    data,
    loading,
    error,
    execute,
    reset,
  }
}

// 自動執行的 API Hook
export function useApiEffect<TData, TParams = void>(
  apiCall: (params: TParams) => Promise<TData>,
  params: TParams,
  deps: React.DependencyList = []
) {
  const { data, loading, error, execute } = useApi(apiCall)

  useEffect(() => {
    execute(params)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps)

  return { data, loading, error, refetch: () => execute(params) }
}
