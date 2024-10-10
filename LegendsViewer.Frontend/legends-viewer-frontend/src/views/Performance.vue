<script setup lang="ts">
import { usePerformanceStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = usePerformanceStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'performance'">
    </WorldObjectPage>
</template>

<style scoped></style>