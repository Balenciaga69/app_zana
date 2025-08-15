import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tsconfigPaths from 'vite-tsconfig-paths'
import fs from 'fs'
import path from 'path'
import { fileURLToPath } from 'url'

const __dirname = path.dirname(fileURLToPath(import.meta.url))

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tsconfigPaths()],
  server: {
    open: true,
    port: 7414,
    host: '0.0.0.0',
    https: {
      key: fs.readFileSync(path.resolve(__dirname, 'ssl/localhost.key')),
      cert: fs.readFileSync(path.resolve(__dirname, 'ssl/localhost.crt')),
    },
    hmr: {
      port: 7414,
      host: 'localhost',
    },
  },
})
