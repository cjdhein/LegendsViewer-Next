<template>
    <v-card
      :title="list.title"
      :subtitle="list.subtitle"
      height="400"
      variant="text"
    >
    <template v-slot:prepend>
        <v-icon class="mr-2" :icon="list.icon" size="32px"></v-icon>
    </template>
      <v-card-text>
        <v-list height="300" scrollable>
          <v-list-item v-for="(link, i) in list?.items" :key="i">
            <v-list-item-title v-html="link" />
          </v-list-item>
        </v-list>
      </v-card-text>
    </v-card>
  </template>
  
  <script lang="ts">
  import { defineComponent, PropType } from 'vue';
  import { LegendLinkListData } from '../types/legends';
  
  export default defineComponent({
    name: 'LegendsCardList',
    props: {
      list: {
        type: Object as PropType<LegendLinkListData>,
        required: true
      }
    }
  });
  </script>
  