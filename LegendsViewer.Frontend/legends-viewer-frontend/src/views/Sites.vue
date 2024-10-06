<script setup lang="ts">
import { useSiteStore } from '../stores/siteStore';
import { LoadItemsOptions } from '../types/legends';

const store = useSiteStore()


const tableHeaders = [
    { title: 'Id', key: 'id', align: 'end' as ("start" | "end" | "center") | undefined },
    { title: 'Name', key: 'name', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Type', key: 'type', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Region', key: 'subtype', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Link', key: 'html', align: 'start' as ("start" | "end" | "center") | undefined },
    { title: 'Events', key: 'eventCount', align: 'end' as ("start" | "end" | "center") | undefined },
]

const loadWorldObjects = async ({ page, itemsPerPage, sortBy }: LoadItemsOptions) => {
    await store.loadOverview(page, itemsPerPage, sortBy)
}

loadWorldObjects({ page: 1, itemsPerPage: store.objectEventsPerPage, sortBy: [] })

</script>

<template>
    <v-row>
        <v-col cols="12">
            <v-card variant="text">
                <v-row align="center" no-gutters>
                    <v-col class="large-icon mr-2" cols="auto">
                        <v-icon icon="mdi-map-marker"></v-icon>
                    </v-col>
                    <v-col>
                        <v-card-title>Sites</v-card-title>
                        <v-card-subtitle class="multiline-subtitle">
                            Sites are diverse inhabited locations, including towns, fortresses, and other settlements,
                            found throughout the world of Dwarf Fortress. Explore their history, structures, and
                            inhabitants.
                        </v-card-subtitle>
                    </v-col>
                </v-row>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col>
            <v-card title="Overview" subtitle="Browse and search all world sites" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-card-search-outline" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <v-data-table-server v-model:items-per-page="store.objectsPerPage" :headers="tableHeaders"
                        :items="store.objects" :items-length="store.objectsTotalItems" :loading="store.isLoading"
                        item-value="name" @update:options="loadWorldObjects">
                        <template v-slot:item.html="{ value }">
                            <span v-html="value"></span>
                        </template>
                    </v-data-table-server>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
</template>

<style scoped>
.multiline-subtitle {
  white-space: normal;
}
</style>