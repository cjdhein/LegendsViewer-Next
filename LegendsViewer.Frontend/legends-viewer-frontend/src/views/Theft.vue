<script setup lang="ts">
import { useTheftStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useTheftStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'theft'">
    </WorldObjectPage>
</template>

<style scoped></style>