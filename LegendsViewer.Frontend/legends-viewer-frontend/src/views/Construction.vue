<script setup lang="ts">
import { useConstructionStore } from '../stores/worldObjectStores';
import { useConstructionMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useConstructionStore()
const mapStore = useConstructionMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'construction'">
    </WorldObjectPage>
</template>

<style scoped></style>