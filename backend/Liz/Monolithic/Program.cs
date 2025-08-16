/** Most Common Command
dotnet ef migrations add "250815_01"
dotnet csharpier . --config-path "../.csharpierrc"
 */

using System.Reflection;
using FluentValidation;
using Monolithic.Features.Communication;
using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Extensions;
using Monolithic.Shared.Middleware;
using Serilog;
using HealthCheckOptions = Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions;

// 讀取 Serilog 設定
var builder = WebApplication.CreateBuilder(args);

// Serilog 設定
builder.AddSerilogLogging();

// 註冊 PostgreSQL DbContext
builder.Services.AddPostgresDbContext(builder.Configuration);

// 連線字串注入
var connectionString = builder.Configuration.GetConnectionString("UserDbConnection");
var redisConnection = builder.Configuration.GetConnectionString("UserRedis");

// JWT 驗證
builder.Services.AddJwtAuthentication(builder.Configuration);

// Redis 服務註冊
builder.Services.AddRedisServices(builder.Configuration);

// MassTransit 服務註冊
builder.Services.AddMassTransitServices(builder.Configuration);

// 統一 Logger 服務註冊
builder.Services.AddAppLogging();

// 註冊健康檢查
builder.Services.AddAppHealthChecks(builder.Configuration);

// MediatR 服務註冊
builder.Services.AddMediatRServices();

// User Feature 服務註冊
builder.Services.AddUserServices();

// Communication 註冊 (SignalR)
builder.Services.AddCommunicationServices(builder.Configuration);

// FluentValidation 註冊
// GetExecutingAssembly() 自動掃描當前組件中的所有 Validator

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// 讀取 CORS 來源
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(allowedOrigins ?? Array.Empty<string>())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// .NET Core 原生註冊
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

// 註冊全域 Filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseResultFilter>();
});

var app = builder.Build();

// 註冊全域 Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// 自動遷移資料庫
app.Services.MigrateDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors(); // 必須在 UseAuthentication 之前
app.UseAuthentication();
app.UseAuthorization();

// 回報所有健康檢查結果，通常用於一般監控或手動檢查。
app.MapHealthChecks("/health");

// 只回報標記為 "ready" 的檢查，讓平台判斷服務是否「已準備好」接收流量（如資料庫連線、外部依賴都正常），避免流量導入未初始化完成的服務。
app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") }
);

// 用於「活存性」檢查（liveness probe），確保服務進程還活著。若回傳 unhealthy，平台會自動重啟服務，避免服務掛死卻無人察覺。
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

app.MapControllers();

// 註冊 Communication Hub (SignalR)
app.MapHub<CommunicationHub>("/communication-hub");

app.Run();
