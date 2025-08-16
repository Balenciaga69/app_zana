import FingerprintJS from '@fingerprintjs/fingerprintjs'

/**
 * Helper for getting device fingerprint (with localStorage cache)
 */
export class DeviceFingerprintHelper {
  private static cacheKey = 'deviceFingerprint'

  /**
   * 取得裝置指紋（有快取）
   */
  static async getFingerprint(): Promise<string> {
    // 先從 localStorage 取
    const cached = localStorage.getItem(this.cacheKey)
    if (cached) return cached

    // 產生新指紋
    const fp = await FingerprintJS.load()
    const result = await fp.get()
    const visitorId = result.visitorId
    localStorage.setItem(this.cacheKey, visitorId)
    return visitorId
  }

  /**
   * 清除快取（如需重設）
   */
  static clearCache() {
    localStorage.removeItem(this.cacheKey)
  }
}
