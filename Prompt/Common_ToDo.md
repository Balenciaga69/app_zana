# 已完成事項
## 前端開發
- 新增四個基本頁面： | 首頁 | 加入房間頁 | 建立房間頁 | 聊天室頁
## 後端開發
- 建立 `DbContext`
- Migrate 資料庫完成
- 建立後端單元測試專案
- 建立統一的 ApiResponse 格式 並且實作了 Middleware 與 Filter 統一處理
- 
## 維運
- 建立 GitHub Actions，自動格式化 PR 的 YAML

# 未完成事項

## 後端開發 TODO
- [ ] IdentityService：建立 UserId 生成與管理邏輯
- [ ] RoomService：實作房間的 CRUD（建立、查詢、加入、離開、銷毀）
- [ ] ChatService：訊息儲存、查詢、推送
- [ ] ConnectionService：SignalR 連線管理與狀態追蹤
- [ ] API 設計：規劃 RESTful 路由與統一回應格式
- [ ] 撰寫房間、用戶、訊息等核心 Entity 與 Repository
- [ ] 實作房間人數上限與密碼驗證
- [ ] 斷線重連與對話找回機制
- [ ] 房間無人自動銷毀與訊息清除
- [ ] 日誌與錯誤處理（Serilog、Middleware）
- [ ] 撰寫單元測試（xUnit/Moq）
- [ ] 撰寫 API 文件（Swagger）


