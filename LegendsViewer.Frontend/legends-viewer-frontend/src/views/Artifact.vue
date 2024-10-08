<script setup lang="ts">
import { useArtifactStore } from '../stores/worldObjectStores';
import { useArtifactMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useArtifactStore()
const mapStore = useArtifactMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'artifact'">
    </WorldObjectPage>
</template>

<style scoped></style>