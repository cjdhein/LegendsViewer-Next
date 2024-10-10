<script setup lang="ts">
import { usePurgeStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = usePurgeStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'purge'">
    </WorldObjectPage>
</template>

<style scoped></style>