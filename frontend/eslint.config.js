import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'
import { globalIgnores } from 'eslint/config'

export default tseslint.config([
  globalIgnores(['dist']),
  {
    files: ['**/*.{ts,tsx}'],
    extends: [
      js.configs.recommended,
      tseslint.configs.recommended,
      reactHooks.configs['recommended-latest'],
      reactRefresh.configs.vite,
      // ESLint 推薦的基礎規則
      'eslint:recommended',
      // React 推薦規則
      'plugin:react/recommended',
      // React Hooks 推薦規則
      'plugin:react-hooks/recommended',
      // TypeScript 推薦規則
      'plugin:@typescript-eslint/recommended',
      // 啟用 eslint-plugin-prettier 並禁用與 Prettier 衝突的規則
      'plugin:prettier/recommended',
      // JSX 可訪問性推薦規則
      'plugin:jsx-a11y/recommended',
    ],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
  },
])
