<script setup lang="ts">
import { useHistoricalFigureStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import FamilyTree from '../components/FamilyTree.vue';

const store = useHistoricalFigureStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'hf'">
        <template v-if="store.object?.familyTreeData" v-slot:type-specific-before-table>
            <v-col cols="12" xl="4" lg="6" md="12">
                <v-card title="Family Tree" subtitle="Ancestry and Descendants" height="400" variant="text">
                    <template v-slot:prepend>
                        <v-icon class="mr-2" icon="mdi-family-tree" size="32px"></v-icon>
                    </template>
                    <v-card-text>
                        <FamilyTree :key="store.object.id" :data="store.object.familyTreeData as any" />
                    </v-card-text>
                    <template v-slot:append>
                        <v-dialog>
                            <template v-slot:activator="{ props: activatorProps }">
                                <v-btn v-bind="activatorProps" icon="mdi-resize" size="large"></v-btn>
                            </template>

                            <template v-slot:default="{ isActive }">
                                <v-card title="Family Tree" subtitle="Ancestry and Descendants">
                                    <template v-slot:prepend>
                                        <v-icon class="mr-2" icon="mdi-family-tree" size="32px"></v-icon>
                                    </template>
                                    <v-card-text>
                                        <FamilyTree :key="-(store.object.id ?? 0)" :data="store.object.familyTreeData as any" :fullscreen="true" />
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
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>