<script setup lang="ts">
import { useEntityStore } from '../stores/worldObjectStores';
import { useEntityMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
import { computed, ComputedRef } from 'vue';
import { LegendLinkListData } from '../types/legends';
import DirectedChordDiagram from '../components/DirectedChordDiagram.vue';

const store = useEntityStore()
const mapStore = useEntityMapStore()

const beforeLists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Wars', items: store.object?.warList ?? [], icon: "mdi-sword-cross", subtitle: "Wars shaping the realm’s history" },
    { title: 'Noble Positions', items: store.object?.entityPositionAssignmentsList ?? [], icon: "mdi-seal", subtitle: "The ruling elite, guiding the fate of realms and people" },
    { title: 'Related Factions and Groups', items: store.object?.entityEntityLinkList ?? [], icon: "mdi-account-group", subtitle: "The organizations and groups connected to this entity" },
    { title: 'Current Sites', items: store.object?.currentSiteList ?? [], icon: "mdi-home-outline", subtitle: "The sites held by this entity, from settlements to strongholds of power" },
    { title: 'Lost Sites', items: store.object?.lostSiteList ?? [], icon: "mdi-home-off-outline", subtitle: "Former strongholds and settlements once under the control of this entity" },
]);

const afterLists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Related Sites', items: store.object?.entitySiteLinkList ?? [], icon: "mdi-home-switch-outline", subtitle: "The locations tied to this entity, from settlements to strongholds of power" },
    { title: 'Worshipped Deities', items: store.object?.worshippedLinks ?? [], icon: "mdi-weather-sunset", subtitle: "The divine beings revered across the lands" },
]);

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'entity'">
        <template v-slot:type-specific-before-table>
            <v-col v-if="store.object?.warDiagramData != null && store.object?.warDiagramData.length > 0" cols="12"
                xl="4" lg="6" md="12">
                <v-card title="War Graph" subtitle="Scaled representation of faction roles in war" height="400"
                    variant="text">
                    <template v-slot:prepend>
                        <v-icon class="mr-2" icon="mdi-family-tree" size="32px"></v-icon>
                    </template>
                    <v-card-text>
                        <div style="width: 320px; margin: auto;">
                            <DirectedChordDiagram :data="store.object?.warDiagramData ?? []" :font-size="32" />
                        </div>
                    </v-card-text>
                    <template v-slot:append>
                        <v-dialog>
                            <template v-slot:activator="{ props: activatorProps }">
                                <v-btn v-bind="activatorProps" icon="mdi-resize" size="large"></v-btn>
                            </template>

                            <template v-slot:default="{ isActive }">
                                <v-card title="War Graph" subtitle="Scaled representation of faction roles in war">
                                    <template v-slot:prepend>
                                        <v-icon class="mr-2" icon="mdi-family-tree" size="32px"></v-icon>
                                    </template>
                                    <v-card-text style="background-color: rgb(var(--v-theme-background));">
                                        <div style="width: 760px; margin: auto;">
                                            <DirectedChordDiagram :data="store.object?.warDiagramData ?? []"
                                                :font-size="16" />
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
            <template v-for="(list, i) in beforeLists" :key="i">
                <v-col v-if="list?.items.length" cols="12" xl="4" lg="6" md="12">
                    <LegendsCardList :list="list" />
                </v-col>
            </template>
        </template>
        <template v-slot:type-specific-after-table>
            <template v-for="(list, i) in afterLists" :key="i">
                <v-col v-if="list?.items.length" cols="12" xl="4" lg="6" md="12">
                    <LegendsCardList :list="list" />
                </v-col>
            </template>
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>