# 已完成事項
## 前端開發
- 新增四個基本頁面： | 首頁 | 加入房間頁 | 建立房間頁 | 聊天室頁
## 後端開發
- 建立 `DbContext`
- Migrate 資料庫完成
- 建立後端單元測試專案
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


---
- 製作統一個 ApiResponse 2xx/4xx/5xx 都會使用該格式回傳
   - 我們來討論格式
   - 橫切面該怎麼處理他
- 製作各種Exception
   - 我們來討論格式
   - 橫切面該怎麼處理他
   - 每種錯誤都該有甚麼特點或不可替代性

好的 我希望你在 在 Shared/Common/ApiResponse.cs 建立上述類別。
建立 ErrorHandlingMiddleware，捕捉所有未處理例外，統一回傳 ApiResponse。
Controller 只需回傳資料，包裝交由 Filter/Middleware 處理。可用 ActionFilter 或 ResultFilter 統一包裝成功回應。


