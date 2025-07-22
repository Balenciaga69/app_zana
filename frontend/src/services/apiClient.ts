// API 客戶端基礎設定
import axios, { AxiosInstance, AxiosRequestConfig } from 'axios'
import { config } from '@/config/config'

// 建立 Axios 實例
const createApiClient = (baseURL: string): AxiosInstance => {
  const client = axios.create({
    baseURL,
    timeout: 10000,
    headers: {
      'Content-Type': 'application/json',
    },
  })

  // 請求攔截器
  client.interceptors.request.use(
    (config) => {
      // 可以在這裡添加認證 token
      const token = localStorage.getItem('authToken')
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
      return config
    },
    (error) => Promise.reject(error)
  )

  // 響應攔截器
  client.interceptors.response.use(
    (response) => response,
    (error) => {
      // 統一錯誤處理
      if (error.response?.status === 401) {
        // 處理未授權
        localStorage.removeItem('authToken')
        // 可以觸發登出或重新導向
      }
      return Promise.reject(error)
    }
  )

  return client
}

// 主要 API 客戶端
export const apiClient = createApiClient(config.api.baseUrl || 'http://localhost:5219')

// API 呼叫的通用工具函數
export const createApiCall = <TData = any, TParams = any>(
  requestConfig: (params: TParams) => AxiosRequestConfig
) => {
  return async (params: TParams): Promise<TData> => {
    const response = await apiClient(requestConfig(params))
    return response.data
  }
}
