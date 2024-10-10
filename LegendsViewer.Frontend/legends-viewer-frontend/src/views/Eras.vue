<script setup lang="ts">
import { useEraStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends';

const store = useEraStore();
const icon = "mdi-timelapse";
const title = "Eras";
const subtitle = "The defining periods that shaped the course of history and civilization";
const overviewSubtitle = "Browse and search all eras";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Start', key: 'type', align: 'start' },
    { title: 'End', key: 'subtype', align: 'end' },
    { title: 'Name', key: 'html', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>