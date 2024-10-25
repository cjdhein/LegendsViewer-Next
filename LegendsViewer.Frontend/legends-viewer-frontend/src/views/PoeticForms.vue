<script setup lang="ts">
import { usePoeticFormStore } from '../stores/worldObjectStores';
import WorldObjectsPage from '../components/WorldObjectsPage.vue';
import { TableHeader } from '../types/legends';

const store = usePoeticFormStore();
const icon = "mdi-account-voice";
const title = "Poetic Forms";
const subtitle = "The art of words, capturing the essence of emotion, story, and tradition in verse";
const overviewSubtitle = "Browse and search all poetic forms";
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