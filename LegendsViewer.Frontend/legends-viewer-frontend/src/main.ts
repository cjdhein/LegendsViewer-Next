import { createApp } from 'vue'
import './style.css'

// Components
import App from './App.vue'

// Vue Router
import { createMemoryHistory, createRouter } from 'vue-router'
import WorldOverviewView from './components/WorldOverview.vue'

const routes = [
  { path: '/', component: WorldOverviewView },
]

const router = createRouter({
  history: createMemoryHistory(),
  routes,
})

// Pinia
import { createPinia } from 'pinia'
const pinia = createPinia()

// Vuetify
import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css' // Ensure you are using css-loader
import { createVuetify } from 'vuetify'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: {
      mdi,
    },
  },
  theme: {
    defaultTheme: 'dark'
  }
})

createApp(App)
  .use(router)
  .use(pinia)
  .use(vuetify)
  .mount('#app')