<script setup lang="ts">
import { ref } from 'vue';
import { useBookmarkStore } from '../stores/bookmarkStore';

const bookmarkStore = useBookmarkStore()
bookmarkStore.getAll()

const fileInput = ref<HTMLInputElement | null>(null);

// Trigger file selection dialog
const openFileDialog = () => {
  fileInput.value?.click();
};

// Handle file selection and process file
const onFileSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    const file = target.files[0];
    const filePath = file.name; // Get file name (not full path)
    console.log(filePath)
    
    // Assuming you want to send file path or file name to the store
    bookmarkStore.load(filePath);
  }
};

// Function to prepare a proper base64 string for png images
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
          <v-btn
           v-if="bookmark.filePath && bookmark.state !== 'Loaded'"
           :loading="bookmark.state === 'Loading'"
           color="blue"
           text="Load"
           @click="bookmarkStore.load(bookmark.filePath)">
          </v-btn>
          <v-btn
           v-if="bookmark.filePath && bookmark.state === 'Loaded'"
           color="green-lighten-2"
           text="Explore"
           @click="console.log(bookmark.filePath)">
          </v-btn>

          <v-spacer></v-spacer>
          <v-chip>
            {{ bookmark.worldWidth + " x " + bookmark.worldHeight }}
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
          <v-btn
            color="orange-lighten-2"
            text="Select"
            @click="openFileDialog">
          </v-btn>

          <v-spacer></v-spacer>

        </v-card-actions>
      </v-card>
      <!-- Hidden file input to open the file dialog -->
      <input
        ref="fileInput"
        type="file"
        accept=".xml"
        style="display: none;"
        @change="onFileSelected"
      />
    </v-col>
  </v-row>
</template>
