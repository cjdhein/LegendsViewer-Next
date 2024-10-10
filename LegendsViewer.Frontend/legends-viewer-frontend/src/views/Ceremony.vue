<script setup lang="ts">
import { useCeremonyStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useCeremonyStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'ceremony'">
    </WorldObjectPage>
</template>

<style scoped></style>