// SignalR/通用錯誤訊息擷取工具
export function extractErrorMessage(err: unknown, fallback: string): string {
  if (
    err &&
    typeof err === 'object' &&
    'message' in err &&
    typeof (err as { message?: unknown }).message === 'string'
  ) {
    return (err as { message: string }).message || fallback
  }
  return fallback
}
