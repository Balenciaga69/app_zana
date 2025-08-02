/** Most Common Command
dotnet ef migrations add "2025-05-02-1" `
  --project .\UserService\UserService.Infra\UserService.Infra.csproj `
  --startup-project .\UserService\UserService.API\UserService.API.csproj
 dotnet csharpier . --config-path "./.csharpierrc"
 */

using Liz.Monolithic;
using Liz.Monolithic.Infrastructure.Extensions;
using Monolithic.Infrastructure.Data;
using Serilog;

// 讀取 Serilog 設定
var builder = WebApplication.CreateBuilder(args);

// Serilog 設定
builder.AddSerilogLogging();

// 連線字串注入
var connectionString = builder.Configuration.GetConnectionString("UserDbConnection");
var redisConnection = builder.Configuration.GetConnectionString("UserRedis");

// JWT 驗證
builder.Services.AddJwtAuthentication(builder.Configuration);

// RabbitMQ 設定注入
builder.Services.AddRabbitMqOptions(builder.Configuration);

// .NET Core 原生註冊
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
