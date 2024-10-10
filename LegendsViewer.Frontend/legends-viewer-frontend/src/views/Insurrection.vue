<script setup lang="ts">
import { useInsurrectionStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useInsurrectionStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'insurrection'">
    </WorldObjectPage>
</template>

<style scoped></style>