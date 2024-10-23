<script setup lang="ts">
import { useArtifactStore } from '../stores/worldObjectStores';
import { useArtifactMapStore } from '../stores/mapStore';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useArtifactStore()
const mapStore = useArtifactMapStore()

</script>

<template>
    <WorldObjectPage :store="store" :mapStore="mapStore" :object-type="'artifact'">
        <template v-slot:type-specific-before-table>
            <v-col cols="12" xl="4" lg="6" md="12">
                <!-- Additional Infos -->
                <v-card max-height="400" variant="text">
                    <v-card-text>
                        <v-list lines="two" class="mt-10" style="background-color: rgb(var(--v-theme-background));" density="compact">
                            <v-list-item
                                v-if="store.object?.subtype != null && store.object?.subtype.length > 0"
                                title="Type">
                                <v-list-item-subtitle>
                                    {{ store.object?.subtype }}
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item v-if="store.object?.holderLink" title="Current Holder">
                                <v-list-item-subtitle>
                                    <div v-html="store.object?.holderLink"></div>
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item v-if="store.object?.structureLink" title="Stored in Structure">
                                <v-list-item-subtitle>
                                    <div v-html="store.object?.structureLink"></div>
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item v-if="store.object?.siteLink" title="Stored at Site">
                                <v-list-item-subtitle>
                                    <div v-html="store.object?.siteLink"></div>
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item v-if="store.object?.regionLink" title="Region">
                                <v-list-item-subtitle>
                                    <div v-html="store.object?.regionLink"></div>
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item v-if="store.object?.creatorLink" title="Creator">
                                <v-list-item-subtitle>
                                    <div v-html="store.object?.creatorLink"></div>
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item v-if="store.object?.writtenContentLink" title="Written Content">
                                <v-list-item-subtitle>
                                    <div v-html="store.object?.writtenContentLink"></div>
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item
                                v-if="store.object?.pageCount != null && store.object?.pageCount > 0"
                                title="Pages">
                                <v-list-item-subtitle>
                                    {{ store.object?.pageCount }}
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item
                                v-if="store.object?.item != null && store.object?.item.length > 0 && store.object?.item !== store.object?.name && store.object?.item !== '«untitled»'"
                                title="Original Name">
                                <v-list-item-subtitle>
                                    {{ store.object?.item }}
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item
                                v-if="store.object?.material != null && store.object?.material.length > 0"
                                title="Material">
                                <v-list-item-subtitle>
                                    {{ store.object?.material }}
                                </v-list-item-subtitle>
                            </v-list-item>
                            <v-list-item
                                v-if="store.object?.description != null && store.object?.description.length > 0"
                                title="Description">
                                <v-list-item-subtitle>
                                    {{ store.object?.description }}
                                </v-list-item-subtitle>
                            </v-list-item>
                        </v-list>
                    </v-card-text>
                </v-card>
            </v-col>
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>