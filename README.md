# app_zana

## 中文簡介 | Chinese Description

**app_zana** 是一個基於 ChatGPT/Gemini API 的多人即時聊天室專案，支援匿名用戶參與回合制協作討論。

### 專案特色
- 🚀 **免登入匿名聊天** - 無需註冊即可快速加入討論
- 🏠 **房間系統** - 支援創建與加入私人聊天室
- 🔒 **密碼保護** - 房間可設定密碼與人數上限（1-10人）
- 💬 **即時通訊** - 基於 SignalR 的低延遲訊息傳遞
- 🔄 **斷線重連** - 自動恢復連線並保留聊天記錄
- 🤖 **AI 整合** - 未來整合 ChatGPT/Gemini API 進行智慧互動

### 技術棧
**前端:** React + TypeScript + Vite + Chakra UI + SignalR Client  
**後端:** .NET 8 + SignalR + EF Core + PostgreSQL + Redis  
**DevOps:** Docker + GitHub Actions

### 開發階段
- **Jackson 階段** (進行中): 基礎聊天室功能實現
- **Paul 階段** (規劃中): AI 整合與遊戲模板
- **Anderson 階段** (未來): 內容審查與防灌水機制

---

## English Description

**app_zana** is a real-time multi-user chat application powered by ChatGPT/Gemini API, supporting anonymous collaborative discussions in a turn-based format.

### Key Features
- 🚀 **Anonymous Chat** - Join discussions instantly without registration
- 🏠 **Room System** - Create and join private chat rooms
- 🔒 **Password Protection** - Rooms support password and participant limits (1-10 users)
- 💬 **Real-time Messaging** - Low-latency communication via SignalR
- 🔄 **Auto-reconnect** - Automatic connection recovery with chat history preservation
- 🤖 **AI Integration** - Future integration with ChatGPT/Gemini API for intelligent interactions

### Tech Stack
**Frontend:** React + TypeScript + Vite + Chakra UI + SignalR Client  
**Backend:** .NET 8 + SignalR + EF Core + PostgreSQL + Redis  
**DevOps:** Docker + GitHub Actions

### Development Phases
- **Jackson Phase** (Current): Core chat room functionality
- **Paul Phase** (Planned): AI integration and game templates
- **Anderson Phase** (Future): Content moderation and spam prevention

---

## 快速開始 | Quick Start

### 前置需求 | Prerequisites
- Node.js 18+
- .NET 8 SDK
- Docker & Docker Compose
- PostgreSQL (或使用 Docker)


## 專案結構 | Project Structure

```
app_zana/
├── frontend/          # React 前端應用
├── backend/           # .NET 後端服務
│   └── Liz/          # 主要後端專案
├── Legacy/           # 舊版實驗代碼
├── Prompt/           # 開發指南與規範
└── scripts/          # 部署與維護腳本
```
