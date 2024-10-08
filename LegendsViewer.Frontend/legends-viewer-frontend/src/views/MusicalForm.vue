<script setup lang="ts">
import { useMusicalFormStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useMusicalFormStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'musicalform'">
    </WorldObjectPage>
</template>

<style scoped></style>