<template>
    <v-fab class="me-2" icon="mdi-chevron-right" location="top end" absolute :to="'/' + objectType + '/' + (routeId + 1)">
    </v-fab>
    <v-fab class="me-16" icon="mdi-chevron-left" location="top end" absolute :to="'/' + objectType + '/' + (routeId - 1)">
    </v-fab>
    <v-row>
        <v-col cols="12">
            <v-card variant="text">
                <v-row align="center" no-gutters>
                    <v-col class="large-icon" cols="auto">
                        <div v-html="store.object?.icon"></div>
                    </v-col>
                    <v-col>
                        <v-card-title>{{ store.object?.name ?? '' }}</v-card-title>
                        <v-card-subtitle class="multiline-subtitle">
                            {{ store.object?.type ?? '' }}
                        </v-card-subtitle>
                    </v-col>
                </v-row>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="mapStore?.currentWorldObjectMap" cols="12" xl="4" lg="6" md="12">
            <!-- Location on World Map -->
            <v-card title="Location" :subtitle="'The location of ' + store.object?.name + ' on the world map'"
                height="400" variant="text" to="/map">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-map-search-outline" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <v-img width="320" height="320" class="position-relative ml-12" :src="mapStore.currentWorldObjectMap"
                        :cover="false" />
                </v-card-text>
            </v-card>
        </v-col>
        <slot name="type-specific-before-table"></slot>
    </v-row>
    <v-row>
        <v-col v-if="store.object?.eventCount != null && store.object?.eventCount > 0">
            <v-card title="Events" :subtitle="'A timeline of events for ' + store.object?.name" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-calendar-clock" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <LineChart v-if="store.objectEventChartData != null" :chart-data="store.objectEventChartData" />
                    <v-data-table-server v-model:items-per-page="store.objectEventsPerPage" :headers="eventTableHeaders" :items="store.objectEvents"
                    :items-length="store.objectEventsTotalItems" :loading="store.isLoading" item-value="name"
                    @update:options="loadEvents">
                    <template v-slot:item.html="{ value }">
                        <span v-html="value"></span>
                    </template>
                </v-data-table-server>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="store.object?.eventCollectionCount != null && store.object?.eventCollectionCount > 0">
            <v-card title="Chronicles" :subtitle="'A list of chronicles for ' + store.object?.name" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-calendar-clock" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <v-data-table-server v-model:items-per-page="store.objectEventCollectionsPerPage" :headers="eventCollectionTableHeaders" :items="store.objectEventCollections"
                    :items-length="store.objectEventCollectionsTotalItems" :loading="store.isLoading" item-value="name"
                    @update:options="loadEventCollections">
                    <template v-slot:item.subtype="{ value }">
                        <span v-html="value"></span>
                    </template>
                    <template v-slot:item.html="{ value }">
                        <span v-html="value"></span>
                    </template>
                </v-data-table-server>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <slot name="type-specific-after-table"></slot>
    </v-row>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue';
import { useRoute } from 'vue-router';
import { LoadItemsOptions, TableHeader } from '../types/legends';
import LineChart from '../components/LineChart.vue';

const route = useRoute()
const routeId = computed(() => {
    if (typeof route.params.id === 'string') {
        return parseInt(route.params.id, 10)
    }
    return 0;
});

const loadEvents = async ({ page, itemsPerPage, sortBy }: LoadItemsOptions) => {
    await props.store.loadEvents(routeId.value, page, itemsPerPage, sortBy)
}

const loadEventCollections = async ({ page, itemsPerPage, sortBy }: LoadItemsOptions) => {
    await props.store.loadEventCollections(routeId.value, page, itemsPerPage, sortBy)
}

const eventTableHeaders = [
    { title: 'Date', key: 'date' },
    { title: 'Type', key: 'type' },
    { title: 'Event', key: 'html' },
]

const eventCollectionTableHeaders: TableHeader[] = [
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Name', key: 'name', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Subtype', key: 'subtype', align: 'start' },
    { title: 'Link', key: 'html', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
]

const props = defineProps({
    store: {
        type: Object,
        required: true,
    },
    mapStore: {
        type: Object,
        required: false,
    },
    objectType: {
        type: String,
        required: true,
    },
});

const load = async (idString: string | string[]) => {
    if (typeof idString === 'string') {
        const id = parseInt(idString, 10)
        await props.store.load(id)
        await props.mapStore?.loadWorldObjectMap(id, 'Default')
        await props.store.loadEventChartData(id)
        await loadEvents({ page: 1, itemsPerPage: props.store.objectEventsPerPage, sortBy: [] })
    }
}

load(route.params.id)


watch(
    () => route.params.id,
    load
)

</script>

<style scoped>
</style>
