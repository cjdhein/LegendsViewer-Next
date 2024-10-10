<script setup lang="ts">
import { useJourneyStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useJourneyStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'journey'">
    </WorldObjectPage>
</template>

<style scoped></style>