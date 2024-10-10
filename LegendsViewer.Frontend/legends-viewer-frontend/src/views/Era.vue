<script setup lang="ts">
import { useEraStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useEraStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'era'">
    </WorldObjectPage>
</template>

<style scoped></style>