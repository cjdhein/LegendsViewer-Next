<script setup lang="ts">
import { useLandmassStore } from '../stores/worldObjectStores';
import { useLandmassMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useLandmassStore()
const mapStore = useLandmassMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'landmass'">
    </WorldObjectPage>
</template>

<style scoped></style>