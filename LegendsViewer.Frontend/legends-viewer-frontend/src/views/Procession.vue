<script setup lang="ts">
import { useProcessionStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useProcessionStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'procession'">
    </WorldObjectPage>
</template>

<style scoped></style>