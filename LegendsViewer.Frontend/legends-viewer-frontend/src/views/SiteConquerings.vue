<script setup lang="ts">
import { useSiteConqueredStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends'; // Adjust the import path as needed

const store = useSiteConqueredStore();
const icon = "mdi-flag-variant";
const title = "Site Conquerings";
const subtitle = "The capture and occupation of critical strongholds and cities";
const overviewSubtitle = "Browse and search all site conquerings";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [ // Specify the type here
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Name', key: 'name', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Outcome', key: 'subtype', align: 'start' },
    { title: 'Link', key: 'html', align: 'start' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>