<script setup lang="ts">
import { usePerformanceStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends';

const store = usePerformanceStore();
const icon = "mdi-drama-masks";
const title = "Performances";
const subtitle = "An artistic display, from music to theater, bringing culture and creativity to life";
const overviewSubtitle = "Browse and search all performances";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [
    { title: 'Start', key: 'startDate', align: 'center' },
    { title: 'End', key: 'endDate', align: 'center' },
    { title: 'Name', key: 'html', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Civilization', key: 'subtype', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>