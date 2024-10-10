<script setup lang="ts">
import { useTheftStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends';

const store = useTheftStore();
const icon = "mdi-handcuffs";
const title = "Thefts";
const subtitle = "Stealthy acts of taking property, from common goods to priceless artifacts";
const overviewSubtitle = "Browse and search all thefts";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [
    { title: 'Start', key: 'startDate', align: 'center' },
    { title: 'End', key: 'endDate', align: 'center' },
    { title: 'Name', key: 'html', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Attacker vs. Defender', key: 'subtype', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>