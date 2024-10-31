<script setup lang="ts">
import { useWrittenContentStore } from '../stores/worldObjectStores';
import WorldObjectPage from '../components/WorldObjectPage.vue';
import LegendsCardList from '../components/LegendsCardList.vue';
import { computed, ComputedRef } from 'vue';
import { LegendLinkListData } from '../types/legends';

const store = useWrittenContentStore()

const lists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Overview', items: store.object?.miscList ?? [], icon: "mdi-book", subtitle: "A summary of significant details of this written content" },
]);

</script>

<template>
    <WorldObjectPage :store="store" :object-type="'writtencontent'">
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