<script setup lang="ts">
import { ref } from 'vue';
import { LoadItemsOptionsWithSearch } from '../types/legends';
import { useSiteStore } from '../stores/worldObjectStores';

const store = useSiteStore()
const icon = "mdi-map-marker"
const title = "Sites"
const subtitle = "Sites are diverse inhabited locations, including towns, fortresses, and other settlements, found throughout the world. Explore their history, structures, and inhabitants"
const overviewSubtitle = "Browse and search all world sites"
const tableHeaders = [
    { title: 'Id', key: 'id', align: 'end' as ("start" | "end" | "center") | undefined },
    { title: 'Name', key: 'name', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Type', key: 'type', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Region', key: 'subtype', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Link', key: 'html', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Events', key: 'eventCount', align: 'end' as ("start" | "end" | "center") | undefined },
]

const searchString = ref("")
const loadWorldObjects = async ({ page, itemsPerPage, sortBy, search }: LoadItemsOptionsWithSearch) => {
    await store.loadOverview(page, itemsPerPage, sortBy, search)
}
loadWorldObjects({ page: 1, itemsPerPage: store.objectEventsPerPage, sortBy: [], search: searchString.value })

</script>

<template>
    <v-row>
        <v-col cols="12">
            <v-card variant="text">
                <v-row align="center" no-gutters>
                    <v-col class="large-icon mr-2" cols="auto">
                        <v-icon :icon="icon"></v-icon>
                    </v-col>
                    <v-col>
                        <v-card-title>{{ title }}</v-card-title>
                        <v-card-subtitle class="multiline-subtitle">
                            {{ subtitle }}
                        </v-card-subtitle>
                    </v-col>
                </v-row>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col>
            <v-card title="Overview" :subtitle="overviewSubtitle" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-card-search-outline" size="32px"></v-icon>
                </template>
                <template v-slot:text>
                    <v-text-field v-model="searchString" label="Search" prepend-inner-icon="mdi-magnify"
                        variant="outlined" hide-details single-line></v-text-field>
                </template>
                <v-card-text>
                    <v-data-table-server v-model:items-per-page="store.objectsPerPage" :headers="tableHeaders"
                        :items="store.objects"
                        :items-length="store.objectsTotalFilteredItems"
                        :search="searchString"
                        :loading="store.isLoading"
                        item-value="name"
                        @update:options="loadWorldObjects">
                        <template v-slot:item.html="{ value }">
                            <span v-html="value"></span>
                        </template>
                    </v-data-table-server>
                </v-card-text>
                <template v-slot:append>
                    <v-chip class="ma-2" color="cyan" label>
                        <v-icon :icon="icon" start></v-icon>
                        {{ title }} Count: {{ store.objectsTotalItems }}
                    </v-chip>
                </template>
            </v-card>
        </v-col>
    </v-row>
</template>

<style scoped>
.multiline-subtitle {
    white-space: normal;
}
</style>