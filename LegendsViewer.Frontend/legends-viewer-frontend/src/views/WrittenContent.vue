<script setup lang="ts">
import { useWrittenContentStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useWrittenContentStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'writtencontent'">
    </WorldObjectPage>
</template>

<style scoped></style>