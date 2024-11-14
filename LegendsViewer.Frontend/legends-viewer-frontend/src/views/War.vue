<script setup lang="ts">
import { useWarStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
import ExpandableCard from '../components/ExpandableCard.vue';
import DoughnutChart from '../components/DoughnutChart.vue';
import { computed, ComputedRef } from 'vue';
import { LegendLinkListData } from '../types/legends';
import DirectedChordDiagram from '../components/DirectedChordDiagram.vue';

const store = useWarStore()


const lists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'War Overview', items: store.object?.miscList ?? [], icon: "mdi-swords-crossed", subtitle: "A summary of significant details of this war" },
    { title: 'Battles', items: store.object?.battleList ?? [], icon: "mdi-chess-bishop", subtitle: "Battles shaping the realm’s history" },
    { title: 'Notable Deaths', items: store.object?.notableDeathLinks ?? [], icon: "mdi-grave-stone", subtitle: "Key figures who died in this war" },
]);

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'war'">
        <template v-slot:type-specific-before-table>
            <v-col v-if="store.object?.battleDiagramData != null && store.object?.battleDiagramData.length > 0" cols="12"
                xl="4" lg="6" md="12">
                <ExpandableCard title="Battle Graph" subtitle="Scaled representation of faction roles in battle"
                    icon="mdi-sword-cross">
                    <template #compact-content>
                        <div style="width: 320px; margin: auto;">
                            <DirectedChordDiagram :key="store.object.id" :data="store.object?.battleDiagramData ?? []" :font-size="32" />
                        </div>
                    </template>
                    <template #expanded-content>
                        <div style="width: 760px; margin: auto;">
                            <DirectedChordDiagram :key="store.object.id" :data="store.object?.battleDiagramData ?? []" :font-size="16" />
                        </div>
                    </template>
                </ExpandableCard>
            </v-col>
            <template v-for="(list, i) in lists" :key="i">
                <v-col v-if="list?.items.length" cols="12" xl="4" lg="6" md="12">
                    <LegendsCardList :list="list" />
                </v-col>
            </template>
           <v-col v-if="store.object?.deathsByRace?.labels != null && store.object?.deathsByRace?.labels?.length > 0"
                cols="12" xl="4" lg="6" md="12">
                <v-card title="Deaths by Race" :subtitle="'How many deaths were in this war, grouped by the race'"
                    height="400" variant="text">
                    <template v-slot:prepend>
                        <v-icon class="mr-2" icon="mdi-grave-stone" size="32px"></v-icon>
                    </template>
                    <v-card-text>
                        <DoughnutChart :chart-data="store.object?.deathsByRace" />
                    </v-card-text>
                </v-card>
            </v-col>
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>