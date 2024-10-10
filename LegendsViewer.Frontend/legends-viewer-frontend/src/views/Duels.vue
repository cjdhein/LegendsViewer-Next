<script setup lang="ts">
import { useDuelStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends'; // Adjust the import path as needed

const store = useDuelStore();
const icon = "mdi-fencing";
const title = "Duels";
const subtitle = "Personal combat between champions, where honor and skill are tested";
const overviewSubtitle = "Browse and search all duels";
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