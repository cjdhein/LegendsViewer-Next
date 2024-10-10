<script setup lang="ts">
import { useAbductionStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useAbductionStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'abduction'">
    </WorldObjectPage>
</template>

<style scoped></style>