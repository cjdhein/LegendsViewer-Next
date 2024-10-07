<script setup lang="ts">
import { useRiverStore } from '../stores/worldObjectStores';
import { useRiverMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useRiverStore()
const mapStore = useRiverMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'river'">
    </WorldObjectPage>
</template>

<style scoped></style>