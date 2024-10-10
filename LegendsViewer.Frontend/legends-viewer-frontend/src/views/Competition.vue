<script setup lang="ts">
import { useCompetitionStore} from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useCompetitionStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'competition'">
    </WorldObjectPage>
</template>

<style scoped></style>