import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', name: 'Overview', component: () => import('../views/WorldOverview.vue') },
  { path: '/map', name: 'Map', component: () => import('../views/Map.vue') },
  { path: '/site', name: 'Sites', component: () => import('../views/Sites.vue') },
  { path: '/site/:id', name: 'Site', component: () => import('../views/Site.vue') },
  { path: '/region', name: 'Regions', component: () => import('../views/Regions.vue') },
  { path: '/region/:id', name: 'Region', component: () => import('../views/Region.vue') },
  { path: '/uregion', name: 'Underground Regions', component: () => import('../views/UndergroundRegions.vue') },
  { path: '/uregion/:id', name: 'Underground Region', component: () => import('../views/UndergroundRegion.vue') },
  { path: '/landmass', name: 'Landmasses', component: () => import('../views/Landmasses.vue') },
  { path: '/landmass/:id', name: 'Landmass', component: () => import('../views/Landmass.vue') },
  { path: '/river', name: 'Rivers', component: () => import('../views/Rivers.vue') },
  { path: '/river/:id', name: 'River', component: () => import('../views/River.vue') },
  { path: '/structure', name: 'Structures', component: () => import('../views/Structures.vue') },
  { path: '/structure/:id', name: 'Structure', component: () => import('../views/Structure.vue') },
  { path: '/construction', name: 'Constructions', component: () => import('../views/Constructions.vue') },
  { path: '/construction/:id', name: 'Construction', component: () => import('../views/Construction.vue') },
  { path: '/mountainpeak', name: 'Mountain Peaks', component: () => import('../views/MountainPeaks.vue') },
  { path: '/mountainpeak/:id', name: 'Mountain Peak', component: () => import('../views/MountainPeak.vue') },
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