<script setup lang="ts">
import { useDuelStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useDuelStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'duel'">
    </WorldObjectPage>
</template>

<style scoped></style>