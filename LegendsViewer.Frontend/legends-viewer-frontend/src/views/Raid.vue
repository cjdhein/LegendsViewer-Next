<script setup lang="ts">
import { useRaidStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useRaidStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'raid'">
    </WorldObjectPage>
</template>

<style scoped></style>