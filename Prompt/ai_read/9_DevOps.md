# DevOps 練習計畫

本專案目標：以低成本（或免費）在 AWS 與 GitHub 上實踐現代前端專案的 DevOps 流程，並可供 LLM 讀取作為自動化操作指令。(儘管我沒有 FreeTier)

## 工具與平台

- 版本控制：GitHub
- CI/CD：GitHub Actions
- 代碼品質檢查：ESLint
- 人工製品儲存：GitHub Releases
- 容器化：Docker
- 靜態網站部署：GitHub Pages、AWS S3 + CloudFront

## 流程規劃

1. 代碼推送至 GitHub。
2. 觸發 GitHub Actions：
   - 執行 ESLint 進行代碼品質檢查。
   - Build 前端專案。
   - 產生 build 產物並上傳至 GitHub Releases。
   - 建立 Docker 映像檔並推送至 GitHub Packages（或 AWS ECR）。
3. 部署：
   - 靜態網站自動部署至 GitHub Pages。
   - 同步 build 產物至 AWS S3，並透過 CloudFront 提供 CDN 加速。
## 分支規則
- Main 是真的有功能可以用而且沒問題
- Develop 是開發中的功能，可能有 Bug
- Feature 分支用於開發新功能，完成後合併至 Develop
- Feature 分支命名規則：`feature/功能名稱/...(/可選)`
- Feature -> Develop 需要經過`auto-format`腳本
- Develop -> Main 需要經過 CI/CD 腳本
## 成本與注意事項

- 優先使用 GitHub 免費資源，AWS 僅用於 S3/CloudFront 並控制於免費額度內。
- 練習完畢請務必關閉 AWS 資源，避免產生額外費用。

---

> 本文件為 LLM Prompt，請依據上述規劃自動化執行相關 DevOps 流程。
