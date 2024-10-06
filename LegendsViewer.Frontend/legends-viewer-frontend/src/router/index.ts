import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', name: 'Overview', component: () => import('../views/WorldOverview.vue') },
  { path: '/map', name: 'Map', component: () => import('../views/Map.vue') },
  { path: '/site', name: 'Sites', component: () => import('../views/Sites.vue') },
  { path: '/site/:id', name: 'Site', component: () => import('../views/Site.vue') },
  { path: '/region', name: 'Regions', component: () => import('../views/Regions.vue') },
  { path: '/uregion', name: 'Underground Regions', component: () => import('../views/UndergroundRegions.vue') },
  { path: '/landmass', name: 'Landmasses', component: () => import('../views/Landmasses.vue') },
  { path: '/river', name: 'Rivers', component: () => import('../views/Rivers.vue') },
  { path: '/structure', name: 'Structures', component: () => import('../views/Structures.vue') },
  { path: '/construction', name: 'Constructions', component: () => import('../views/Constructions.vue') },
  { path: '/mountainpeak', name: 'Mountain Peaks', component: () => import('../views/MountainPeaks.vue') },
  { path: '/entity', name: 'Entities', component: () => import('../views/Entities.vue') },
  { path: '/hf', name: 'Historical Figures', component: () => import('../views/HistoricalFigures.vue') },
  { path: '/artifact', name: 'Artifacts', component: () => import('../views/Artifacts.vue') },
  { path: '/danceform', name: 'Dance Forms', component: () => import('../views/DanceForms.vue') },
  { path: '/musicalform', name: 'Musical Forms', component: () => import('../views/MusicalForms.vue') },
  { path: '/poeticform', name: 'Poetic Forms', component: () => import('../views/PoeticForms.vue') },
  { path: '/writtencontent', name: 'Written Content', component: () => import('../views/WrittenContents.vue') },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router;