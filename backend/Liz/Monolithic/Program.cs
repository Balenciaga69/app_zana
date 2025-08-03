/** Most Common Command
dotnet ef migrations add "250803_2"
dotnet csharpier . --config-path "../.csharpierrc"
 */

using Liz.Monolithic.Infrastructure.Extensions;
using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Extensions;
using Monolithic.Shared.Middleware;
using Serilog;

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

// RabbitMQ 設定注入
builder.Services.AddRabbitMqOptions(builder.Configuration);

// Identity 服務註冊
builder.Services.AddIdentityServices();

// 統一 Logger 服務註冊
builder.Services.AddAppLogging();

// .NET Core 原生註冊
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
