<script setup lang="ts">
const menuItems = [
      { title: 'World Overview', icon: 'mdi-file-tree-outline', to: '/' },
      { title: 'Map', icon: 'mdi-map-search-outline', to: '/map' },
    ];
</script>

<template>
  <v-responsive class="border rounded">
    <v-app>
      <v-app-bar>
        <div class="logo">
          <v-img src="book.png"></v-img>
        </div>
        <h1>
          Legends Viewer
        </h1>
      </v-app-bar>

      <v-navigation-drawer>
        <v-list>
          <v-list-item
            v-for="item in menuItems"
            :key="item.title"
            :to="item.to"
            :active-class="'v-list-item--active'"
          >
            <template v-slot:prepend>
              <v-icon :icon="item.icon"></v-icon>
            </template>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item>
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
  width: 32px;
}
</style>
