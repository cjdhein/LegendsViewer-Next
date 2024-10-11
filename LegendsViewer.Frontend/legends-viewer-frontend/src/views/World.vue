<script setup lang="ts">
import { useWorldStore } from '../stores/worldStore';
import { useWorldMapStore } from '../stores/mapStore';
import DoughnutChart from '../components/DoughnutChart.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
import { computed, ComputedRef } from 'vue';
import { LegendLinkListData } from '../types/legends';

const store = useWorldStore()
const mapStore = useWorldMapStore()

store.loadWorld();
mapStore.loadWorldMap('Default');

const lists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Heroic Ties', items: store.world?.playerRelatedObjects ?? [], icon: "mdi-compass-outline", subtitle: "Discover the adventurers, factions, and locations tied to your journey" },
]);


</script>

<template>
    <v-row>
        <v-col cols="12">
            <v-card variant="text">
                <v-row align="center" no-gutters>
                    <v-col class="large-icon" cols="auto">
                        <v-icon icon="mdi-earth-box" />
                    </v-col>
                    <v-col>
                        <v-card-title>{{ store.world?.name ?? '' }}</v-card-title>
                        <v-card-subtitle class="multiline-subtitle">
                            {{ store.world?.alternativeName ?? '' }}
                        </v-card-subtitle>
                    </v-col>
                </v-row>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="mapStore?.worldMapMid" cols="12" xl="4" lg="6" md="12">
            <!-- World Map -->
            <v-card title="World Overview Map"
                subtitle="Click to explore the interactive map and dive into the world's geography" height="400"
                variant="text" to="/map">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-map-search-outline" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <v-img width="320" height="320" class="position-relative ml-12" :src="mapStore.worldMapMid"
                        :cover="false" />
                </v-card-text>
            </v-card>
        </v-col>
        <v-col
            v-if="store.world?.entityPopulationsByRace?.labels != null && store.world?.entityPopulationsByRace?.labels?.length > 0"
            cols="12" xl="4" lg="6" md="12">
            <v-card title="Population by Race"
                subtitle="A demographic breakdown of the population of the main civilizations" height="400"
                variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-chart-donut" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <DoughnutChart :chart-data="store.world?.entityPopulationsByRace" />
                </v-card-text>
            </v-card>
        </v-col>
        <v-col
            v-if="store.world?.areaByOverworldRegions?.labels != null && store.world?.areaByOverworldRegions?.labels?.length > 0"
            cols="12" xl="4" lg="6" md="12">
            <v-card title="Area by Overworld Regions"
                subtitle="A comparative view of the land distribution across the world" height="400"
                variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-earth" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <DoughnutChart :chart-data="store.world?.areaByOverworldRegions" />
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <template v-for="(list, i) in lists" :key="i">
            <v-col v-if="list?.items.length" cols="12" xl="4" lg="6" md="12">
                <LegendsCardList :list="list" />
            </v-col>
        </template>
    </v-row>
</template>

<style scoped></style>