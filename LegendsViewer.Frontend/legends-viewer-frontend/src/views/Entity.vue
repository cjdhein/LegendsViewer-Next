<script setup lang="ts">
import { useEntityStore } from '../stores/worldObjectStores';
import { useEntityMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
import { computed, ComputedRef } from 'vue';
import { LegendLinkListData } from '../types/legends';

const store = useEntityStore()
const mapStore = useEntityMapStore()

const lists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Noble Positions', items: store.object?.entityPositionAssignmentsList ?? [], icon: "mdi-seal", subtitle: "The ruling elite, guiding the fate of realms and people" },
    { title: 'Worshipped Deities', items: store.object?.worshippedLinks ?? [], icon: "mdi-weather-sunset", subtitle: "The divine beings revered across the lands" },
]);

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'entity'">
        <template v-slot:type-specific-before-table>
            <template v-for="(list, i) in lists" :key="i">
                <v-col v-if="list?.items.length" cols="12" xl="4" lg="6" md="12">
                    <LegendsCardList :list="list" />
                </v-col>
            </template>
        </template>
   </WorldObjectPage>
</template>

<style scoped></style>