import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  build:{
    outDir: `C:\\Program Files (x86)\\Steam\\steamapps\\common\\Dwarf Fortress\\LegendsViewer\\legends-viewer-frontend\\dist`,
    emptyOutDir: true,
  },
  plugins: [vue()],
})
