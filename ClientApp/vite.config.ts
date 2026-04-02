import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  const defaultProxyTarget = 'https://localhost:7048';
  const apiProxyTarget = env.VITE_API_PROXY_TARGET || defaultProxyTarget;

  return {
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
  }
})
