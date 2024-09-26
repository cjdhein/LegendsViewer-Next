<script setup lang="ts">
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
          <RouterLink to="/"><v-list-item>
              <template v-slot:prepend>
                <v-icon icon="mdi-file-tree-outline"></v-icon>
              </template>
              <v-list-item-title>World Overview</v-list-item-title>
            </v-list-item>
          </RouterLink>
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
