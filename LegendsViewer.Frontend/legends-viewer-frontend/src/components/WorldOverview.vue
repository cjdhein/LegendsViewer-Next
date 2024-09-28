<script setup lang="ts">
import { useBookmarkStore } from '../stores/bookmarkStore';

const bookmarkStore = useBookmarkStore()
bookmarkStore.getAll()

// Function to convert byte[] to base64 string
const getImageData = (bookmark: any) => {
  if (!bookmark.worldMapImage) {
    return ''; // Return an empty string if there's no image data
  }
  return `data:image/png;base64,${bookmark.worldMapImage}`;
}

</script>

<template>
  <v-row dense>
    <v-col v-for="(bookmark, i) in bookmarkStore.bookmarks" :key="i" cols="12" md="4">
      <v-card class="mx-auto" max-width="320">
        <v-container>
          <v-img
            height="300px"
            width="300px"
            :src="getImageData(bookmark)"
            cover
            >
          </v-img>
        </v-container>

        <v-card-title>
          {{ bookmark.worldName }}
        </v-card-title>

        <v-card-subtitle>
          {{ bookmark.worldAlternativeName }}
        </v-card-subtitle>

        <v-card-actions>
          <v-btn color="blue" text="Load"></v-btn>

          <v-spacer></v-spacer>
          <v-chip>
            Chip
          </v-chip>
        </v-card-actions>
      </v-card>
    </v-col>
    <v-col>
      <v-card class="mx-auto" max-width="320">
        <v-container>
          <v-icon icon="mdi-earth-plus" size="300"></v-icon>
        </v-container>

        <v-card-title>
          Explore a new world
        </v-card-title>

        <v-card-subtitle>
          Select an exported legends XML file
        </v-card-subtitle>

        <v-card-actions>
          <v-btn color="orange-lighten-2" text="Explore"></v-btn>

          <v-spacer></v-spacer>

        </v-card-actions>
      </v-card>

    </v-col>
  </v-row>
</template>
