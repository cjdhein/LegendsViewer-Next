<script setup lang="ts">
import { useMusicalFormStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends';

const store = useMusicalFormStore();
const icon = "mdi-music-clef-treble";
const title = "Musical Forms";
const subtitle = "The rhythmic and melodic expressions that resonate through time, shaping culture and emotion";
const overviewSubtitle = "Browse and search all musical forms";
const overviewTitle = "Overview";
const wikiKeyWord = "Musical_form";
const tableHeaders: TableHeader[] = [
    { title: 'Id', key: 'id', align: 'end' },
    { title: 'Name', key: 'html', align: 'start' },
    { title: 'Type', key: 'type', align: 'start' },
    { title: 'Chronicles', key: 'eventCollectionCount', align: 'end' },
    { title: 'Events', key: 'eventCount', align: 'end' },
];
</script>

<template>
    <WorldObjectsPage :store="store" :icon="icon" :title="title" :subtitle="subtitle" :overviewTitle="overviewTitle"
        :overviewSubtitle="overviewSubtitle" :wikiKeyWord="wikiKeyWord" :tableHeaders="tableHeaders">
    </WorldObjectsPage>
</template>