<script setup lang="ts">
import { useHistoricalFigureStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useHistoricalFigureStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'hf'">
    </WorldObjectPage>
</template>

<style scoped></style>