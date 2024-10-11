<script setup lang="ts">
import { useRegionStore } from '../stores/worldObjectStores';
import { useRegionMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
import { computed, ComputedRef } from 'vue';
import { LegendLinkListData } from '../types/legends';

const store = useRegionStore()
const mapStore = useRegionMapStore()


const beforeLists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Related Sites', items: store.object?.siteLinks ?? [], icon: "mdi-home-switch-outline", subtitle: "The sites in this region, from small settlements to strongholds of power" },
]);

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'region'">
        <template v-slot:type-specific-before-table>
            <template v-for="(list, i) in beforeLists" :key="i">
                    <v-col v-if="list?.items.length" cols="12" xl="4" lg="6" md="12">
                        <LegendsCardList :list="list" />
                    </v-col>
                </template>
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>