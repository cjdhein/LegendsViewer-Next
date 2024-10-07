<script setup lang="ts">
import { useMountainPeakStore } from '../stores/worldObjectStores';
import { useMountainPeakMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useMountainPeakStore()
const mapStore = useMountainPeakMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'mountainpeak'">
    </WorldObjectPage>
</template>

<style scoped></style>