<script setup lang="ts">
import { useStructureStore } from '../stores/worldObjectStores';
import { useStructureMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useStructureStore()
const mapStore = useStructureMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'structure'">
    </WorldObjectPage>
</template>

<style scoped></style>