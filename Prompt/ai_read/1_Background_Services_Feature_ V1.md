# Background Services Feature V1

## 概述

負責系統自動化維護、清理和監控的背景服務集合。確保系統長期穩定運行和資源合理利用。

## 服務列表

### 1. RoomCleanupService（房間清理服務）

#### 職責

- 自動檢測和清理空房間
- 處理非活躍房間的生命週期
- 維護房間狀態的一致性

#### 執行時機
```
CheckIntervalMinutes = 10
InactiveThresholdMinutes = 30
```
- 檢查頻率：每 $CheckIntervalMinutes 分鐘執行一次
- 清理條件：房間無成員且超過 $InactiveThresholdMinutes 分鐘未活動
- 通知機制：清理前透過 SignalR 通知相關用戶

#### 清理流程

1. 查詢所有 `IsActive = true` 且 `LastActiveAt < $InactiveThresholdMinutes 分鐘前` 的房間
2. 檢查房間是否有在線成員（查詢 RoomParticipant 表）
3. 標記房間 `IsActive = false`，設定 `DestroyedAt = DateTime.UtcNow`
4. 透過 SignalR 發送 `RoomDestroyed` 事件給最後的參與者
5. 記錄清理日誌

#### 配置參數

```json
{
  "RoomCleanup": {
    "CheckIntervalMinutes": 10,
    "InactiveThresholdMinutes": 30,
    "BatchSize": 100
  }
}
```

### 2. UserConnectionCleanupService（用戶連線清理服務）

#### 職責

- 清理殭屍連線記錄
- 維護用戶在線狀態準確性
- 釋放無效的 SignalR 連線資源

#### 執行時機
```
CheckIntervalHours = 1
RetentionHours = 24
```
- 檢查頻率：每 $CheckIntervalHours 小時執行一次
- 清理條件：連線已斷開超過 $RetentionHours 小時
- 狀態同步：更新用戶的最後活動時間

#### 清理流程

1. 查詢所有 `DisconnectedAt IS NOT NULL` 且超過 $RetentionHours 小時的連線
2. 刪除過期的 UserConnection 記錄
3. 更新相關用戶的 `LastActiveAt` 和 `IsActive` 狀態
4. 清理 Redis 中的過期快取資料

#### 配置參數

```json
{
  "ConnectionCleanup": {
    "CheckIntervalHours": 1,
    "RetentionHours": 24,
    "BatchSize": 1000
  }
}
```

### 3. InactiveUserCleanupService（非活躍用戶清理服務）

#### 職責

- 清理長期非活躍的用戶資料
- 保護用戶隱私（自動遺忘機制）
- 控制資料庫成長

#### 執行時機

- 檢查頻率：每日午夜執行
- 清理條件：用戶超過 30 天未活動
- 保留邏輯：保留基本統計資料，清除個人識別資訊

#### 清理流程

1. 查詢 `LastActiveAt < 30天前` 的非活躍用戶
2. 匿名化處理：清空 `Nickname`、`DeviceFingerprint`
3. 標記用戶 `IsActive = false`
4. 保留訊息記錄但匿名化發送者資訊
5. 生成清理報告

#### 配置參數

```json
{
  "UserCleanup": {
    "CheckIntervalHours": 24,
    "InactiveThresholdDays": 30,
    "AnonymizeData": true,
    "BatchSize": 500
  }
}
```

### 4. HealthMonitoringService（健康監控服務）

#### 職責

- 監控系統關鍵指標
- 檢測異常狀況並報警
- 提供系統健康狀態報告

#### 監控指標

- 資料庫連線狀態：PostgreSQL 連線池狀況
- Redis 快取狀態：記憶體使用率、連線數
- SignalR 連線統計：即時連線數、訊息傳輸率
- 系統資源：CPU、記憶體使用率
- 業務指標：活躍房間數、在線用戶數

#### 執行時機

- 檢查頻率：每 5 分鐘執行一次
- 報警閾值：可設定的警告和錯誤閾值
- 健康報告：每小時生成健康狀態報告

#### 配置參數

```json
{
  "HealthMonitoring": {
    "CheckIntervalMinutes": 5,
    "DatabaseTimeoutSeconds": 30,
    "RedisTimeoutSeconds": 10,
    "CpuThresholdPercent": 80,
    "MemoryThresholdPercent": 85
  }
}
```

### 5. MessageArchiveService（訊息歸檔服務）

#### 職責

- 歸檔舊訊息到冷儲存
- 維護訊息查詢效能
- 實現訊息的分層儲存策略

#### 執行時機

- 檢查頻率：每週執行一次
- 歸檔條件：超過 90 天的訊息
- 策略：移動到歸檔表或冷儲存

#### 歸檔流程

1. 識別超過 90 天的訊息記錄
2. 將舊訊息移動到 `MessageArchive` 表
3. 從主要 `Message` 表中刪除已歸檔記錄
4. 更新查詢邏輯以支援歸檔資料查詢
5. 壓縮和優化歸檔資料

#### 配置參數

```json
{
  "MessageArchive": {
    "CheckIntervalDays": 7,
    "ArchiveThresholdDays": 90,
    "BatchSize": 10000,
    "CompressionEnabled": true
  }
}
```

## 基礎設施

### 服務基類

```csharp
public abstract class BackgroundServiceBase : BackgroundService
{
    protected readonly ILogger Logger;
    protected readonly IServiceProvider ServiceProvider;
    
    protected abstract TimeSpan ExecutionInterval { get; }
    protected abstract Task ExecuteTaskAsync(CancellationToken cancellationToken);
}
```

### 錯誤處理

- 異常隔離：單一服務失敗不影響其他服務
- 重試機制：支援自動重試和指數退避
- 錯誤通知：嚴重錯誤自動發送管理員通知

### 監控與日誌

- 執行日誌：記錄每次執行的結果和耗時
- 效能監控：追蹤服務執行效能指標
- 健康狀態：提供服務健康狀態端點

### 配置管理

- 動態配置：支援運行時修改服務參數
- 環境區分：開發、測試、生產環境不同配置
- 驗證機制：配置參數的有效性檢查

## 部署與維護

### Docker 支援

- 背景服務與 Web API 共用同一容器
- 支援獨立部署背景服務容器
- 使用 Docker Compose 統一管理

### 擴展性

- 支援多實例執行（使用分散式鎖）
- 防止重複執行的協調機制
- 負載均衡和故障轉移

### 運維工具

- 手動觸發：提供管理員手動執行服務的介面
- 執行報告：詳細的執行結果和統計報告
- 性能調優：根據系統負載動態調整執行頻率

## 未來擴展

### 智能清理

- 基於使用模式的智能清理策略
- 機器學習預測最佳清理時機
- 用戶行為分析優化服務參數

### 分散式架構

- 支援 Kubernetes Job 執行
- 使用消息隊列協調服務執行
- 微服務架構下的服務分離
