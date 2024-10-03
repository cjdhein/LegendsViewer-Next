import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', name: 'Overview', component: () => import('../views/WorldOverview.vue') },
  { path: '/map', name: 'Map', component: () => import('../views/Map.vue') },
  { path: '/site/:id', name: 'Site', component: () => import('../views/Site.vue') },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router;