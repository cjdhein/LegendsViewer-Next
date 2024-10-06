<script setup lang="ts">
const menuItems = [
      { title: 'Explore Worlds', icon: 'mdi-file-tree-outline', to: '/' },
      { type: 'subheader', title: 'Societies and Figures' },
      { title: 'Entities', icon: 'mdi-account-group', to: '/entity' },
      { title: 'Historical Figures', icon: 'mdi-account', to: '/hf' },
      { type: 'subheader', title: 'Geography' },
      { title: 'Map', icon: 'mdi-map-search-outline', to: '/map' },
      { title: 'Regions', icon: 'mdi-map-legend', to: '/region' },
      { title: 'Underground', icon: 'mdi-tunnel', to: '/uregion' },
      { title: 'Landmasses', icon: 'mdi-island-variant', to: '/landmass' },
      { title: 'Rivers', icon: 'mdi-waves', to: '/river' },
      { title: 'Mountain Peaks', icon: 'mdi-summit', to: '/mountainpeak' },
      { type: 'subheader', title: 'Infrastructure' },
      { title: 'Sites', icon: 'mdi-home-modern', to: '/site' },
      { title: 'Structures', icon: 'mdi-home-silo', to: '/structure' },
      { title: 'Constructions', icon: 'mdi-sign-caution', to: '/construction' },
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
        <v-list>
          <template v-for="(item, i) in menuItems" :key="i">
            <v-list-subheader v-if="item.type == 'subheader'">{{ item.title.toUpperCase() }}</v-list-subheader>
            <v-list-item v-else
              :prepend-icon="item.icon"
              :title="item.title"
              :to="item.to"
              :active-class="'v-list-item--active'"
            />
          </template>
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
