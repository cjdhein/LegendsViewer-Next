<script setup lang="ts">
import { useMusicalFormStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import DescriptionCard from '../components/DescriptionCard.vue';

const store = useMusicalFormStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'musicalform'">
        <template v-slot:type-specific-before-table>
            <v-col v-if="store.object?.description != null"
                cols="12" xl="6" lg="12">
                <DescriptionCard :description="store.object.description"></DescriptionCard>
            </v-col>
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>