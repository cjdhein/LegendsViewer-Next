<template>
    <v-card title="Description" subtitle="An in-depth look at the origins, techniques, and cultural significance"
        height="400" variant="text">
        <template v-slot:prepend>
            <v-icon class="mr-2" icon="mdi-book-open-page-variant" size="32px"></v-icon>
        </template>
        <v-card-text class="ml-12 scrollable-description">
            <div v-html="description.replace(/ {2}/g, '<br/>').replace(/\[B\]/g, '<li>')"></div>
        </v-card-text>
    </v-card>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue';
  
  export default defineComponent({
    name: 'DescriptionCard',
    props: {
      description: {
        type: String,
        required: true
      }
    }
  });
  </script>
  
<style scoped>
.scrollable-description {
  max-height: 300px; /* Set a max height for the scrollable area */
  overflow-y: auto; /* Enable vertical scrolling */
  padding: 16px; /* Add some padding for readability */
  line-height: 1.6; /* Improve readability with line height */
  text-align: justify; /* Align text for easier reading in prose form */
}
</style>