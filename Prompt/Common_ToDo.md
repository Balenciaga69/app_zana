# 已完成事項

## 前端開發

- 新增四個基本頁面： | 首頁 | 加入房間頁 | 建立房間頁 | 聊天室頁
- 包含:
  - 房間首頁
  - 建立房間頁
  - 加入房間頁
  - 聊天室頁
- 使用 Chakra UI 進行樣式設計
- 使用 Zustand 管理全域狀態
- 使用 React Router 進行路由管理
- 使用 React Hook Form 處理表單
- 使用 Axios 進行 API 請求
- 使用 SignalR Client 進行即時通訊

## 後端開發

- 建立 `DbContext`、`Entities`
- Migrate 資料庫完成
- 建立後端單元測試專案
- 建立統一的 ApiResponse 格式 並且實作了 Middleware 與 Filter 統一處理
- 新增統一 Logger 服務 (強化 +1)
- 新增健康檢查 (強化 +3)
- ~~進度 30% 建立 Identity 服務與瀏覽器指紋功能(預計全面棄用重構)~~ **已棄用，重新設計**

## 維運

- 建立 GitHub Actions，自動格式化 PR 的 YAML

# 分階段開發計劃 🚀

## 🎯 階段 1：User Feature 核心功能（Priority: HIGH）
> **目標：建立匿名用戶系統，支援設備指紋識別**
> **預估時間：3-5 天**

### 1.1 更新 Entity 結構
- [ ] **修正 User Entity**
  - 將 `Id` 改為 `string` 類型（GUID 字串）
  - 移除不需要的 `Nickname` 必填限制
  - 確保符合計畫書規格

- [ ] **完善 UserConnection Entity**
  - 確認 SignalR ConnectionId 追蹤功能
  - 加入 IP 和 UserAgent 記錄

### 1.2 實作 User Feature
- [ ] **User Commands**
  - `RegisterUserCommand` - 新用戶註冊/現有用戶識別
  - `UpdateNicknameCommand` - 更新用戶暱稱
  - `UpdateUserActivityCommand` - 更新活動時間

- [ ] **User Queries**
  - `GetUserByIdQuery` - 根據 UserId 查詢用戶
  - `GetUserByDeviceFingerprintQuery` - 根據設備指紋查詢用戶
  - `GetOnlineUsersStatsQuery` - 在線用戶統計

- [ ] **User Repository**
  - 介面與實作放同一檔案
  - 支援設備指紋去重複邏輯

- [ ] **User Controller**
  - GET /users/{id}
  - GET /users/me
  - PUT /users/me/nickname
  - GET /users/online-stats

### 1.3 設備指紋技術
- [ ] **Device Fingerprint Service**
  - 瀏覽器特徵收集
  - 指紋生成演算法
  - 防偽造驗證

### 1.4 單元測試
- [ ] User Commands Handler 測試
- [ ] User Queries Handler 測試
- [ ] Repository 測試
- [ ] Controller 測試

---

## 🎯 階段 2：SignalR Hub 基礎建設（Priority: HIGH）
> **目標：建立 WebSocket 通訊中心**
> **預估時間：4-6 天**

### 2.1 ChatHub 核心功能
- [ ] **連線管理**
  - `RegisterUser(existingUserId?, deviceFingerprint)` 方法
  - `OnConnectedAsync` 生命週期處理
  - `OnDisconnectedAsync` 清理邏輯

- [ ] **用戶狀態管理**
  - `UpdateNickname(newNickname)` 方法
  - `Heartbeat()` 心跳機制
  - 全域在線統計廣播

### 2.2 權限與驗證
- [ ] **連線驗證機制**
  - ConnectionId 與 UserId 綁定驗證
  - 未註冊連線的超時處理
  - 頻率限制實作

### 2.3 錯誤處理與日誌
- [ ] **統一錯誤處理**
  - 標準錯誤碼定義
  - Error 事件廣播
  - 詳細日誌記錄

### 2.4 Redis 整合
- [ ] **快取策略**
  - 用戶連線狀態快取
  - 在線統計快取
  - SignalR Groups 管理

### 2.5 單元測試
- [ ] Hub 方法測試
- [ ] 連線管理測試
- [ ] 權限驗證測試

---

## 🎯 階段 3：Room Feature 實作（Priority: HIGH）
> **目標：房間管理系統**
> **預估時間：5-7 天**

### 3.1 Room Commands
- [ ] `CreateRoomCommand` - 建立房間
- [ ] `UpdateRoomSettingsCommand` - 更新房間設定
- [ ] `JoinRoomCommand` - 加入房間（含密碼驗證）
- [ ] `LeaveRoomCommand` - 離開房間

### 3.2 Room Queries
- [ ] `GetRoomByIdQuery` - 查詢房間資訊
- [ ] `GetRoomByInviteCodeQuery` - 透過邀請碼查詢
- [ ] `GetRoomParticipantsQuery` - 查詢房間成員
- [ ] `GetUserRoomsQuery` - 查詢用戶參與的房間

### 3.3 Room Repository
- [ ] 房間 CRUD 操作
- [ ] 參與者狀態管理
- [ ] 密碼 Hash 處理

### 3.4 Room Controller
- [ ] POST /rooms - 建立房間
- [ ] GET /rooms/{id} - 查詢房間
- [ ] PUT /rooms/{id} - 更新房間（房主限定）
- [ ] GET /rooms/invite/{inviteCode} - 邀請碼查詢

### 3.5 SignalR Room 功能
- [ ] **Hub 方法擴展**
  - `JoinRoom(roomId, password?)` 
  - `LeaveRoom(roomId)`
  - 即時成員進出通知

### 3.6 單元測試
- [ ] Room 業務邏輯測試
- [ ] 權限控制測試
- [ ] SignalR 房間功能測試

---

## 🎯 階段 4：Message Feature 實作（Priority: MEDIUM）
> **目標：即時訊息系統**
> **預估時間：4-5 天**

### 4.1 Message Commands
- [ ] `SendMessageCommand` - 發送訊息（透過 SignalR）

### 4.2 Message Queries
- [ ] `GetRoomMessagesQuery` - 查詢房間歷史訊息（分頁）
- [ ] `GetMessageByIdQuery` - 查詢單一訊息

### 4.3 Message Repository
- [ ] 訊息持久化
- [ ] 歷史查詢優化
- [ ] 資料庫索引確認

### 4.4 Message Controller
- [ ] GET /rooms/{roomId}/messages - 歷史訊息查詢

### 4.5 SignalR Message 功能
- [ ] **Hub 方法擴展**
  - `SendMessageToRoom(roomId, content)`
  - 訊息廣播邏輯
  - 防濫用機制

### 4.6 單元測試
- [ ] 訊息發送流程測試
- [ ] 權限驗證測試
- [ ] 歷史查詢測試

---

## 🎯 階段 5：Background Services（Priority: MEDIUM）
> **目標：系統自動化維護**
> **預估時間：3-4 天**

### 5.1 核心背景服務
- [ ] **RoomCleanupService**
  - 空房間自動清理
  - 非活躍房間銷毀
  - SignalR 通知機制

- [ ] **UserConnectionCleanupService**
  - 殭屍連線清理
  - 用戶狀態同步
  - Redis 快取清理

### 5.2 服務基礎設施
- [ ] BackgroundService 基類
- [ ] 配置參數管理
- [ ] 錯誤處理與重試

### 5.3 單元測試
- [ ] 清理邏輯測試
- [ ] 配置參數測試

---

## 🎯 階段 6：整合測試與優化（Priority: LOW）
> **目標：系統整體驗證**
> **預估時間：2-3 天**

### 6.1 端到端測試
- [ ] 完整使用者流程測試
- [ ] 多用戶並發測試
- [ ] 斷線重連測試

### 6.2 效能優化
- [ ] 資料庫查詢優化
- [ ] Redis 快取策略調整
- [ ] SignalR 連線管理優化

### 6.3 監控與日誌
- [ ] 關鍵指標收集
- [ ] 錯誤追蹤完善
- [ ] 健康檢查擴展

---

## 🎯 階段 7：未來準備（Priority: FUTURE）
> **目標：微服務化準備**

### 7.1 架構重構準備
- [ ] Feature 邊界清晰化
- [ ] 跨 Feature 依賴梳理
- [ ] API 標準化

### 7.2 部署與運維
- [ ] Docker 優化
- [ ] CI/CD 完善
- [ ] 監控系統整合

---

# 開發建議與注意事項 ⚠️

## 🔥 立即開始：階段 1 首要任務
1. **修正 User Entity** - 確保 Id 為 string 類型
2. **實作 RegisterUserCommand** - 這是所有功能的基礎
3. **建立設備指紋服務** - 匿名用戶識別的核心

## 💡 開發策略
- **TDD 優先**：每個 Handler 都先寫測試
- **小步快跑**：每個階段完成就能運行基本功能
- **垂直切片**：每次都完成一個完整的功能切片
- **測試驅動**：先寫測試，再實作業務邏輯

## 🚨 常見陷阱
- 不要跳階段：確保前一階段穩定再進行下一階段
- 不要過度設計：符合計畫書即可，避免提前優化
- 記住 Entity 不要用 Navigation Property
- SignalR 方法要有適當的權限驗證

## 📋 Daily CheckList
- [ ] 今日目標明確（具體到某個 Handler 或方法）
- [ ] 寫了對應的單元測試
- [ ] 程式碼符合計畫書的原則
- [ ] 有適當的錯誤處理和日誌
- [ ] 更新 TODO 狀態

**開始建議：從階段 1.1 修正 User Entity 開始！** 🚀
