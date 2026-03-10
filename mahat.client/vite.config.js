import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],

  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },

  server: {
    port: 5173,        // Change to any port you want
    strictPort: true,
    https: false,      // <-- Forces HTTP only
    host: true,        // Allows LAN access (optional)
    
    // REMOVE if not needed
    /*
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
      }
    }
    */
  }
});
