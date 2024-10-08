<script setup lang="ts">
import { useEntityStore } from '../stores/worldObjectStores';
import { useEntityMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useEntityStore()
const mapStore = useEntityMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'entity'">
    </WorldObjectPage>
</template>

<style scoped></style>