<script setup lang="ts">
import { useWarStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useWarStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'war'">
    </WorldObjectPage>
</template>

<style scoped></style>