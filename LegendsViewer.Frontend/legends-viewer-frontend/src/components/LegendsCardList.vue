<template>
  <v-card :title="list.title" :subtitle="list.subtitle" height="400" variant="text">
    <template v-slot:prepend>
      <v-icon class="mr-2" :icon="list.icon" size="32px"></v-icon>
    </template>
    <v-card-text>
      <v-list v-if="list?.items?.length > 0" class="ml-12" height="320" lines="two" scrollable>
        <template v-for="(link, i) in list.items" :key="i">
          <template v-if="isString(link)">
            <v-list-item>
              <!-- If the link is a string -->
              <v-list-item-title v-html="link" />
            </v-list-item>
          </template>
          <template v-else>
            <v-list-item>
              <template v-if="link?.prepend" v-slot:prepend>
                    <div v-html="link.prepend"></div>
              </template>
              <template v-if="link?.title" v-slot:title>
                    <div v-html="link.title"></div>
              </template>
              <template v-if="link?.subtitle" v-slot:subtitle>
                    <div v-html="link.subtitle"></div>
              </template>
              <template v-if="link?.append" v-slot:append>
                    <div v-html="link.append"></div>
              </template>
            </v-list-item>
          </template>
        </template>
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