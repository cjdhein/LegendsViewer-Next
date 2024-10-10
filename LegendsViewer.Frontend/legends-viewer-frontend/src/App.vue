<script setup lang="ts">
import { useBookmarkStore } from './stores/bookmarkStore';
const bookmarkStore = useBookmarkStore()
bookmarkStore.getAll()

const societyItems = [
  { title: 'Entities', to: '/entity' },
  { title: 'Historical Figures', to: '/hf' },
];
const geographyItems = [
  { title: 'Regions', to: '/region' },
  { title: 'Underground', to: '/uregion' },
  { title: 'Landmasses', to: '/landmass' },
  { title: 'Rivers', to: '/river' },
  { title: 'Mountain Peaks', to: '/mountainpeak' },
];
const infrastructureItems = [
  { title: 'Sites', to: '/site' },
  { title: 'Structures', to: '/structure' },
  { title: 'Constructions', to: '/construction' },
];
const artItems = [
  { title: 'Artifacts', to: '/artifact' },
  { title: 'Dance Forms', to: '/danceform' },
  { title: 'Musical Forms', to: '/musicalform' },
  { title: 'Poetic Forms', to: '/poeticform' },
  { title: 'Written Content', to: '/writtencontent' },
];
const warfareItems = [
  { title: 'Wars', to: '/war' },
  { title: 'Battles', to: '/battle' },
  { title: 'Duels', to: '/duel' },
  { title: 'Insurrections', to: '/insurrection' },
  { title: 'Persecutions', to: '/persecution' },
  { title: 'Purges', to: '/purge' },
  { title: 'Raids', to: '/raid' },
  { title: 'Site Conquerings', to: '/siteconquered' },
  { title: 'Rampages', to: '/beastattack' },
];
</script>

<template>
  <v-responsive class="border rounded">
    <v-app>
      <v-app-bar>
        <div class="logo">
          <v-img src="/ceretelina.png"></v-img>
        </div>
        <h1>
          Legends Viewer
        </h1>
      </v-app-bar>

      <v-navigation-drawer>
        <v-list nav class="nav-list">
          <v-list-item prepend-icon="mdi-file-tree-outline" title="Explore Worlds" to="/"
            :active-class="'v-list-item--active'" />
          <v-list-item prepend-icon="mdi-map-search-outline" title="Map" to="/map" :active-class="'v-list-item--active'"
            :disabled="bookmarkStore?.isLoaded == false" />
          <v-list-group value="Factions & Figures">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-account-group" title="Factions & Figures" :disabled="bookmarkStore?.isLoaded == false"></v-list-item>
            </template>
            <template v-for="(item, i) in societyItems" :key="i">
              <v-list-item :value="item.title + i" :title="item.title" :to="item.to"
                :disabled="bookmarkStore?.isLoaded == false" />
            </template>
          </v-list-group>
          <v-list-group value="Geography">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-island-variant" title="Geography" :disabled="bookmarkStore?.isLoaded == false"></v-list-item>
            </template>
            <template v-for="(item, i) in geographyItems" :key="i">
              <v-list-item :value="item.title + i" :title="item.title" :to="item.to"
                :disabled="bookmarkStore?.isLoaded == false" />
            </template>
          </v-list-group>
          <v-list-group value="Infrastructure">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-home-modern" title="Infrastructure" :disabled="bookmarkStore?.isLoaded == false"></v-list-item>
            </template>
            <template v-for="(item, i) in infrastructureItems" :key="i">
              <v-list-item :value="item.title + i" :title="item.title" :to="item.to"
                :disabled="bookmarkStore?.isLoaded == false" />
            </template>
          </v-list-group>
          <v-list-group value="Art and Craft">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-diamond-stone" title="Art and Craft" :disabled="bookmarkStore?.isLoaded == false"></v-list-item>
            </template>
            <template v-for="(item, i) in artItems" :key="i">
              <v-list-item :value="item.title + i" :title="item.title" :to="item.to"
                :disabled="bookmarkStore?.isLoaded == false" />
            </template>
          </v-list-group>
          <v-list-group value="Warfare">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-sword-cross" title="Warfare" :disabled="bookmarkStore?.isLoaded == false"></v-list-item>
            </template>
            <template v-for="(item, i) in warfareItems" :key="i">
              <v-list-item :value="item.title + i" :title="item.title" :to="item.to"
                :disabled="bookmarkStore?.isLoaded == false" />
            </template>
          </v-list-group>
        </v-list>
      </v-navigation-drawer>

      <v-main>
        <v-container>
          <RouterView />
        </v-container>
      </v-main>
    </v-app>
  </v-responsive>
</template>

<style scoped>
.logo {
  margin: 12px;
  width: 36px;
}
</style>
