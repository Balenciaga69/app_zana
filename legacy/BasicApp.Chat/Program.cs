using BasicApp.Chat.Extensions;
using BasicApp.Chat.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddConnectionServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:7414").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 啟用 CORS（跨來源資源分享）
app.UseCors();
// 啟用 HTTPS 重新導向
app.UseHttpsRedirection();
// 啟用驗證
app.UseAuthentication();
// 啟用授權
app.UseAuthorization();
// 映射控制器路由
app.MapControllers();
// 映射 SignalR 聊天 Hub
app.MapHub<ChatHub>("/chat");
// 啟動應用程式
app.Run();
