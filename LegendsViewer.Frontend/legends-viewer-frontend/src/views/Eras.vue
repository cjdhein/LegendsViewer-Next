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
const wikiKeyWord = "Ages";
const tableHeaders: TableHeader[] = [
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Duration', key: 'type', align: 'center' },
    { title: 'From - To', key: 'subtype', align: 'center' },
    { title: 'Name', key: 'html', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :wikiKeyWord="wikiKeyWord" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>