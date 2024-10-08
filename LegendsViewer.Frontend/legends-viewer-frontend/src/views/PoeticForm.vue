<script setup lang="ts">
import { usePoeticFormStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = usePoeticFormStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'poeticform'">
    </WorldObjectPage>
</template>

<style scoped></style>