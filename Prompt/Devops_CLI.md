# 常用 DevOps 指令清單

## 前端
- 安裝依賴：`pnpm install`
- 格式檢查：`pnpm run lint`
- 格式化：`pnpm run format`
- 建置：`pnpm run build`

## 後端
- 還原工具：`dotnet tool restore`
- 格式化：`dotnet csharpier . --config-path "./.csharpierrc"`
- 代碼清理：`dotnet format`
- 建置：`dotnet build`

## 其他
- 複雜自動化腳本請見 scripts/ 目錄
