<script setup lang="ts">
import { useWarStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
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
                <v-card title="Battle Graph" subtitle="Scaled representation of faction roles in battle" height="400"
                    variant="text">
                    <template v-slot:prepend>
                        <v-icon class="mr-2" icon="mdi-family-tree" size="32px"></v-icon>
                    </template>
                    <v-card-text>
                        <div style="width: 320px; margin: auto;">
                            <DirectedChordDiagram :data="store.object?.battleDiagramData ?? []" :font-size="32" />
                        </div>
                    </v-card-text>
                    <template v-slot:append>
                        <v-dialog>
                            <template v-slot:activator="{ props: activatorProps }">
                                <v-btn v-bind="activatorProps" icon="mdi-resize" size="large"></v-btn>
                            </template>

                            <template v-slot:default="{ isActive }">
                                <v-card title="Battle Graph" subtitle="Scaled representation of faction roles in battle">
                                    <template v-slot:prepend>
                                        <v-icon class="mr-2" icon="mdi-family-tree" size="32px"></v-icon>
                                    </template>
                                    <v-card-text style="background-color: rgb(var(--v-theme-background));">
                                        <div style="width: 760px; margin: auto;">
                                            <DirectedChordDiagram :data="store.object?.battleDiagramData ?? []"
                                                :font-size="12" />
                                        </div>
                                    </v-card-text>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>

                                        <v-btn text="Close Dialog" @click="isActive.value = false"></v-btn>
                                    </v-card-actions>
                                </v-card>
                            </template>
                        </v-dialog>
                    </template>
                </v-card>
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