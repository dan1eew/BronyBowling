import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

export default defineConfig({
    plugins: [plugin()],
    server: {
      port: 5173,
    }
})
