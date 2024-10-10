<script setup lang="ts">
import { useBeastAttackStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useBeastAttackStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'beastattack'">
    </WorldObjectPage>
</template>

<style scoped></style>