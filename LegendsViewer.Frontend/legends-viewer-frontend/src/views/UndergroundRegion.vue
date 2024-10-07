<script setup lang="ts">
import { useUndergroundRegionStore } from '../stores/worldObjectStores';
import { useUndergroundRegionMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useUndergroundRegionStore()
const mapStore = useUndergroundRegionMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'uregion'">
    </WorldObjectPage>
</template>

<style scoped></style>