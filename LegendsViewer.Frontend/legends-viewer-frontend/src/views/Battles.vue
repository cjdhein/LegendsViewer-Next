<script setup lang="ts">
import { useBattleStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends'; // Adjust the import path as needed

const store = useBattleStore();
const icon = "mdi-chess-bishop";
const title = "Battles";
const subtitle = "Pivotal clashes between armies that decided the fate of nations";
const overviewSubtitle = "Browse and search all battles";
const overviewTitle = "Overview";
const tableHeaders: TableHeader[] = [ // Specify the type here
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Name', key: 'name', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Attacker vs. Defender', key: 'subtype', align: 'start' },
    { title: 'Link', key: 'html', align: 'start' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>