<script setup lang="ts">
import { useDanceFormStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useDanceFormStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'danceform'">
    </WorldObjectPage>
</template>

<style scoped></style>