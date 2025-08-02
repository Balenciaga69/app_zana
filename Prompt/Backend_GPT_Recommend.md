以下是常見且實用的 Entity Framework Core 相關配置，分為「可選」與「建議導入」：
建議導入（實務上很常用，維護性高）
DbSet<TEntity> 屬性

讓每個 Entity 都有 DbSet，方便查詢與操作。
例：public DbSet<Message> Messages { get; set; }
ApplyConfigurationsFromAssembly

將 Fluent API 設定集中到 EntityTypeConfiguration 類別，讓 OnModelCreating 更乾淨。
例：modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);
Transaction 支援

需要多步驟資料一致性時，使用 BeginTransaction/Commit/Rollback。
Query Tracking 行為設定

預設查詢是否追蹤（如 AsNoTracking），提升查詢效能。
例：optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
Global Query Filter

例如軟刪除（IsDeleted）、多租戶（TenantId）等全域過濾條件。
自動審計（CreatedBy/UpdatedBy）

除了時間，也可自動記錄異動人員（需配合登入資訊）。

Seed Data（資料初始化）

OnModelCreating 內用 HasData 預設資料。
自訂 Migration History Table

讓 migration 歷史表有自訂名稱或 schema。
ValueConverter

針對 Enum、複雜型別等自訂轉換。
Shadow Property

不在 Entity class 但存在於資料庫的屬性（如 LastModifiedBy）。
自訂 SaveChanges（同步版）

若有同步 SaveChanges 需求，也可覆寫 public override int SaveChanges()。
DbContext Pooling

在高效能場景下，註冊 AddDbContextPool 以減少 DbContext 建立成本。
自訂資料庫連線字串管理

依環境、租戶動態切換連線字串。
