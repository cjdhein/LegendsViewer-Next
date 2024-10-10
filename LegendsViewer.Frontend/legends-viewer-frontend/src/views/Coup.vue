<script setup lang="ts">
import { useCoupStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useCoupStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'coup'">
    </WorldObjectPage>
</template>

<style scoped></style>