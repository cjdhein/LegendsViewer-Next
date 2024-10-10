<script setup lang="ts">
import { useEntityStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends';

const store = useEntityStore();
const icon = "mdi-account-group";
const title = "Factions and Groups";
const subtitle = "Explore the Influential Factions and Groups Shaping History";
const overviewSubtitle = "Browse and search all entities";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [
{ title: 'Id', key: 'id', align: 'end' },
{ title: 'Name', key: 'html', align: 'start' },
{ title: 'Type', key: 'type', align: 'start' },
    { title: 'Race', key: 'subtype', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>