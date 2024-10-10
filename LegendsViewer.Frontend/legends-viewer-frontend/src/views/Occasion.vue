<script setup lang="ts">
import { useOccasionStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useOccasionStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'occasion'">
    </WorldObjectPage>
</template>

<style scoped></style>