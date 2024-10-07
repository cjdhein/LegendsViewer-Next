<script setup lang="ts">
import { useRegionStore } from '../stores/worldObjectStores';
import { useRegionMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useRegionStore()
const mapStore = useRegionMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'region'">
    </WorldObjectPage>
</template>

<style scoped></style>