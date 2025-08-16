import react from '@vitejs/plugin-react'
import fs from 'fs'
import { defineConfig } from 'vite'
import tsconfigPaths from 'vite-tsconfig-paths'

export default defineConfig({
  plugins: [react(), tsconfigPaths()],
  server: {
    open: true,
    port: 7414,
    host: '0.0.0.0',
    https: {
      key: fs.readFileSync('ssl/localhost-key.pem'),
      cert: fs.readFileSync('ssl/localhost.pem'),
    },
    hmr: {
      port: 7414,
      host: 'localhost',
    },
  },
})
