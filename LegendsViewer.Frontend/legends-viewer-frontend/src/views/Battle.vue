<script setup lang="ts">
import { useBattleStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useBattleStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'battle'">
    </WorldObjectPage>
</template>

<style scoped></style>