import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  root: '.',
  build: {
    outDir: '../wwwroot/kanban',
    emptyOutDir: true,
    rollupOptions: {
      output: {
        entryFileNames: 'kanban.js',
        assetFileNames: 'kanban.[ext]',
      },
    },
  },
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        secure: false,
      },
    },
  },
});
