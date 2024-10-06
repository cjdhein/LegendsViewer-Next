<script setup lang="ts">
import { useSiteStore } from '../stores/worldObjectStores';
import { useRoute } from 'vue-router'
import { computed, ComputedRef, watch } from 'vue'
import { useMapStore } from '../stores/mapStore';
import { LegendLinkListData, LoadItemsOptions } from '../types/legends';
import LegendsCardList from '../components/LegendsCardList.vue';
import DoughnutChart from '../components/DoughnutChart.vue';
import LineChart from '../components/LineChart.vue';

const route = useRoute()
const siteStore = useSiteStore()
const mapStore = useMapStore()

const routeId = computed(() => {
    if (typeof route.params.id === 'string') {
        return parseInt(route.params.id, 10)
    }
    return 0;
});

const loadSiteEvents = async ({ page, itemsPerPage, sortBy }: LoadItemsOptions) => {
    await siteStore.loadEvents(routeId.value, page, itemsPerPage, sortBy)
}

const eventTableHeaders = [
    { title: 'Date', key: 'date' },
    { title: 'Type', key: 'type' },
    { title: 'Event', key: 'html' },
]

const loadSite = async (idString: string | string[]) => {
    if (typeof idString === 'string') {
        const id = parseInt(idString, 10)
        await siteStore.load(id)
        await mapStore.loadWorldSiteMap(id, 'Default')
        await siteStore.loadEventChartData(id)
        await loadSiteEvents({ page: 1, itemsPerPage: siteStore.objectEventsPerPage, sortBy: [] })
    }
}

loadSite(route.params.id)

const lists: ComputedRef<LegendLinkListData[]> = computed(() => [
    { title: 'Structures', items: siteStore.object?.structuresLinks ?? [], icon: "mdi-home-silo", subtitle: "Notable buildings at this site" },
    { title: 'Related Historical Figures', items: siteStore.object?.relatedHistoricalFigureLinks ?? [], icon: "mdi-account-star", subtitle: "Famous figures tied to this site" },
    { title: 'Notable Deaths', items: siteStore.object?.notableDeathLinks ?? [], icon: "mdi-grave-stone", subtitle: "Key figures who died here" },
    { title: 'Battles', items: siteStore.object?.battleLinks ?? [], icon: "mdi-chess-bishop", subtitle: "Battles fought at this site" },
    { title: 'Conquerings', items: siteStore.object?.conqueringLinks ?? [], icon: "mdi-chess-pawn", subtitle: "Conquests that took place here" },
    { title: 'Raids', items: siteStore.object?.raidLinks ?? [], icon: "mdi-lightning-bolt", subtitle: "Raids carried out at this site" },
    { title: 'Duels', items: siteStore.object?.duelLinks ?? [], icon: "mdi-sword-cross", subtitle: "Duels fought at this site" },
    { title: 'Persecutions', items: siteStore.object?.persecutionLinks ?? [], icon: "mdi-map-marker-alert", subtitle: "Acts of persecution at this site" },
    { title: 'Insurrections', items: siteStore.object?.insurrectionLinks ?? [], icon: "mdi-map-marker-alert", subtitle: "Revolts and uprisings here" },
    { title: 'Abductions', items: siteStore.object?.abductionLinks ?? [], icon: "mdi-map-marker-alert", subtitle: "Kidnappings that happened here" },
    { title: 'Beast Attacks', items: siteStore.object?.beastAttackLinks ?? [], icon: "mdi-chess-knight", subtitle: "Beast attacks recorded here" }
]);

watch(
    () => route.params.id,
    loadSite
)

</script>

<template>
    <v-fab class="me-2" icon="mdi-chevron-right" location="top end" absolute :to="'/site/' + (routeId + 1)">
    </v-fab>
    <v-fab class="me-16" icon="mdi-chevron-left" location="top end" absolute :to="'/site/' + (routeId - 1)">
    </v-fab>
    <v-row>
        <v-col cols="12" xl="4" lg="6" md="12">
            <v-card :title="siteStore.object?.name ?? ''" :subtitle="siteStore.object?.type ?? ''" max-height="400"
                variant="text">
                <template v-slot:prepend>
                    <div class="large-icon mr-2" v-html="siteStore.object?.icon"></div>
                </template>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="mapStore.currentWorldSiteMap" cols="12" xl="4" lg="6" md="12">
            <v-card title="Location" :subtitle="'The location of ' + siteStore.object?.name + ' on the world map'"
                height="400" variant="text" to="/map">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-map-search-outline" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <v-img width="320" height="320" class="position-relative ml-12" :src="mapStore.currentWorldSiteMap"
                        :cover="false" />
                </v-card-text>
            </v-card>
        </v-col>
        <v-col cols="12" xl="4" lg="6" md="12">
            <v-card max-height="400" variant="text">
                <v-card-text>
                    <v-list lines="two" class="mt-10" style="background-color: rgb(var(--v-theme-background));">
                        <v-list-item v-if="siteStore.object?.regionToLink" title="Region">
                            <v-list-item-subtitle>
                                <div v-html="siteStore.object?.regionToLink"></div>
                            </v-list-item-subtitle>
                        </v-list-item>
                        <v-list-item v-if="siteStore.object?.currentOwnerToLink" title="Current Owner">
                            <v-list-item-subtitle>
                                <div v-html="siteStore.object?.currentOwnerToLink"></div>
                            </v-list-item-subtitle>
                        </v-list-item>
                        <v-list-item
                            v-if="siteStore.object?.battleLinks != null && siteStore.object?.battleLinks.length > 0"
                            title="Battles">
                            <v-list-item-subtitle>
                                <v-icon icon="mdi-chess-bishop"></v-icon>
                                {{ siteStore.object?.battleLinks.length }}
                            </v-list-item-subtitle>
                        </v-list-item>
                        <v-list-item
                            v-if="siteStore.object?.beastAttackLinks != null && siteStore.object?.beastAttackLinks.length > 0"
                            title="Beast Attacks">
                            <v-list-item-subtitle>
                                <v-icon icon="mdi-chess-knight"></v-icon>
                                {{ siteStore.object?.beastAttackLinks.length }}
                            </v-list-item-subtitle>
                        </v-list-item>
                        <v-list-item
                            v-if="siteStore.object?.notableDeathLinks != null && siteStore.object?.notableDeathLinks.length > 0"
                            title="Notable Deaths">
                            <v-list-item-subtitle>
                                <v-icon icon="mdi-grave-stone"></v-icon>
                                {{ siteStore.object?.notableDeathLinks.length }}
                            </v-list-item-subtitle>
                        </v-list-item>
                    </v-list>
                </v-card-text>
            </v-card>
        </v-col>
        <v-col v-if="siteStore.object?.deathsByRace?.labels != null && siteStore.object?.deathsByRace?.labels?.length > 0"
            cols="12" xl="4" lg="6" md="12">
            <v-card title="Deaths by Race" :subtitle="'How many deaths were at this site, grouped by the race'"
                height="400" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-grave-stone" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <DoughnutChart :chart-data="siteStore.object?.deathsByRace" />
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="siteStore.object?.eventCount != null && siteStore.object?.eventCount > 0">
            <v-card title="Events" subtitle="The events that happend at this site" variant="text">
                <template v-slot:prepend>
                    <v-icon class="mr-2" icon="mdi-calendar-clock" size="32px"></v-icon>
                </template>
                <v-card-text>
                    <LineChart v-if="siteStore.objectEventChartData != null" :chart-data="siteStore.objectEventChartData" />
                    <v-data-table-server v-model:items-per-page="siteStore.objectEventsPerPage" :headers="eventTableHeaders" :items="siteStore.objectEvents"
                    :items-length="siteStore.objectEventsTotalItems" :loading="siteStore.isLoading" item-value="name"
                    @update:options="loadSiteEvents">
                    <template v-slot:item.html="{ value }">
                        <span v-html="value"></span>
                    </template>
                </v-data-table-server>
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