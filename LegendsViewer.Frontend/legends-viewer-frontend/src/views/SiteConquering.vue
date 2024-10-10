<script setup lang="ts">
import { useSiteConqueredStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useSiteConqueredStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'siteconquered'">
    </WorldObjectPage>
</template>

<style scoped></style>