<template>
    <v-card :title="title" :subtitle="subtitle" :height="height" variant="text">
      <template v-slot:prepend>
        <v-icon class="mr-2" :icon="icon" size="32px"></v-icon>
      </template>
  
      <!-- Compact Content Slot -->
      <v-card-text>
        <slot name="compact-content" />
      </v-card-text>
  
      <template v-slot:append>
        <v-dialog v-model="dialogVisible" fullscreen>
          <template v-slot:activator="{ props: activatorProps }">
            <v-btn v-bind="activatorProps" icon="mdi-resize" size="large"></v-btn>
          </template>
  
          <template v-slot:default>
            <v-card :title="title" :subtitle="subtitle">
              <template v-slot:prepend>
                <v-icon class="mr-2" :icon="icon" size="32px"></v-icon>
              </template>
  
              <!-- Expanded Content Slot -->
              <v-card-text>
                <slot name="expanded-content" />
              </v-card-text>
  
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn text="Close Dialog" @click="dialogVisible = false"></v-btn>
              </v-card-actions>
            </v-card>
          </template>
        </v-dialog>
      </template>
    </v-card>
  </template>
  
  <script>
  export default {
    props: {
      title: { type: String, required: true },
      subtitle: { type: String, required: true },
      icon: { type: String, default: "mdi-family-tree" },
      height: { type: String, default: "400" },
    },
    data() {
      return {
        dialogVisible: false,
      };
    },
  };
  </script>
  
  <style scoped>
  /* Add any custom styles here */
  </style>
  