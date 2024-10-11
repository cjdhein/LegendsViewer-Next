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
          <v-list-item v-for="(link, i) in list.items" :key="i">
          <!-- If the link is a string -->
          <v-list-item-title v-if="isString(link)" v-html="link" />
          
          <!-- If the link is a ListItemDto object -->
          <template v-else>
            <v-list-item-icon v-if="link.prepend">
              <v-icon :icon="link.prepend" />
            </v-list-item-icon>
            <v-list-item-title v-html="link.title" />
            <v-list-item-subtitle v-if="link.subtitle" v-html="link.subtitle" />
            <v-list-item-icon v-if="link.append">
              <v-icon :icon="link.append" />
            </v-list-item-icon>
          </template>
        </v-list-item>
        </v-list>
      </v-card-text>
    </v-card>
  </template>
  
  <script lang="ts">
  import { defineComponent, PropType } from 'vue';
  import { LegendLinkListData } from '../types/legends';
  import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

type ListItemDto = components['schemas']['ListItemDto'];

  export default defineComponent({
    name: 'LegendsCardList',
    props: {
      list: {
        type: Object as PropType<LegendLinkListData>,
        required: true
      }
    },
    methods: {
      isString(item: string | ListItemDto): item is string {
        return typeof item === 'string';
      }
    }
  });
  </script>
  