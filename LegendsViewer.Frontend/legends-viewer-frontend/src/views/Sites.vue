
<script setup lang="ts">
import { useSiteStore } from '../stores/siteStore';
import { LoadItemsOptions } from '../types/legends';

const siteStore = useSiteStore()

const loadSites = async ({ page, itemsPerPage, sortBy }: LoadItemsOptions) => {
    await siteStore.loadOverview(page, itemsPerPage, sortBy)
}

const tableHeaders = [
    { title: 'Id', key: 'id' },
    { title: 'Name', key: 'name' },
    { title: 'Type', key: 'type' },
    { title: 'Region', key: 'subtype' },
    { title: 'Link', key: 'html' },
    { title: 'Events', key: 'eventCount' },
]

loadSites({ page: 1, itemsPerPage: siteStore.objectEventsPerPage, sortBy: [] })

</script>

<template>
    <v-row>
        <v-col cols="12" xl="4" lg="6" md="12">
            <v-card title="Sites" max-height="400"
                variant="text">
                <template v-slot:prepend>
                    <div class="large-icon mr-2">
                        <v-icon icon="mdi-map-marker"></v-icon>
                    </div>
                </template>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col>
            <v-card title="Overview" subtitle="Search all the sites in the world" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-card-search-outline" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <v-data-table-server v-model:items-per-page="siteStore.objectsPerPage" :headers="tableHeaders" :items="siteStore.objects"
                    :items-length="siteStore.objectsTotalItems" :loading="siteStore.isLoading" item-value="name"
                    @update:options="loadSites">
                    <template v-slot:item.html="{ value }">
                        <span v-html="value"></span>
                    </template>
                </v-data-table-server>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
</template>

<style scoped></style>