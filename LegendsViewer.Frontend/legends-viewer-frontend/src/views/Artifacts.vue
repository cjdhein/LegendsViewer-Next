<script setup lang="ts">
import { useArtifactStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends'; // Adjust the import path as needed

const store = useArtifactStore();
const icon = "mdi-diamond-stone";
const title = "Artifacts";
const subtitle = "Sites are diverse inhabited locations, including towns, fortresses, and other settlements, found throughout the world. Explore their history, structures, and inhabitants";
const overviewSubtitle = "Browse and search all artifacts";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [ // Specify the type here
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Name', key: 'name', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Subtype', key: 'subtype', align: 'start' },
    { title: 'Link', key: 'html', align: 'start' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>