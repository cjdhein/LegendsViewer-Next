<script setup lang="ts">
import { useLandmassStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends'; // Adjust the import path as needed

const store = useLandmassStore();
const icon = "mdi-island-variant";
const title = "Landmasses";
const subtitle = "Explore the diverse landscapes that shape the world's geography and culture";
const overviewSubtitle = "Browse and search all landmasses";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [ // Specify the type here
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Name', key: 'name', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Link', key: 'html', align: 'start' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>