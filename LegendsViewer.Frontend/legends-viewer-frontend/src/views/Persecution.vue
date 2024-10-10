<script setup lang="ts">
import { usePersecutionStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = usePersecutionStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'persecution'">
    </WorldObjectPage>
</template>

<style scoped></style>