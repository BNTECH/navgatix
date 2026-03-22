import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

const defaultProxyTarget = 'https://localhost:7048';
const apiProxyTarget = (import.meta.env.VITE_API_PROXY_TARGET as string) || defaultProxyTarget;

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: apiProxyTarget,
        changeOrigin: true,
        secure: false,
      },
      '/hubs': {
        target: apiProxyTarget,
        changeOrigin: true,
        secure: false,
        ws: true,
      },
    },
  },
  build: {
    outDir: '../wwwroot',
    emptyOutDir: true,
  },
})
