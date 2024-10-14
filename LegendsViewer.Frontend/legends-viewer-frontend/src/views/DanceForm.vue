<script setup lang="ts">
import { useDanceFormStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';

const store = useDanceFormStore()

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'danceform'">
        <template v-slot:type-specific-before-table>
            <v-col v-if="store.object?.description != null"
                cols="12" xl="4" lg="6" md="12">
                <v-card title="Description" subtitle="..."
                    height="400" variant="text">
                    <template v-slot:prepend>
                        <v-icon class="mr-2" icon="mdi-grave-stone" size="32px"></v-icon>
                    </template>
                    <v-card-text class="ml-12" height="300" scrollable>
                        {{ store.object.description }}
                    </v-card-text>
                </v-card>
            </v-col>
        </template>
    </WorldObjectPage>
</template>

<style scoped></style>