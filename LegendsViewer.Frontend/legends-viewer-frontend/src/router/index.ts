import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', name: 'Overview', component: () => import('../components/WorldOverview.vue') },
  { path: '/map', name: 'Map', component: () => import('../components/Map.vue') },
  { path: '/site/:id', name: 'Site', component: () => import('../components/Site.vue') },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router;