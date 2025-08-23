// SignalR/通用錯誤訊息擷取工具
export function extractErrorMessage(err: unknown, fallback: string): string {
  if (
    err &&
    typeof err === 'object' &&
    'message' in err &&
    typeof (err as { message?: unknown }).message === 'string'
  ) {
    // 回傳錯誤物件中的 message 屬性
    return (err as { message: string }).message || fallback
  }

  // 否則回傳預設錯誤訊息
  return fallback
}
