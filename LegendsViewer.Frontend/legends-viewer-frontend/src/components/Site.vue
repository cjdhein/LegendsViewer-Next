<script setup lang="ts">
import { useSiteStore } from '../stores/siteStore';
import { useRoute } from 'vue-router'
import { computed, watch } from 'vue'
import { useMapStore } from '../stores/mapStore';

const route = useRoute()
const siteStore = useSiteStore()
const mapStore = useMapStore()

const routeId = computed(() => {
    if (typeof route.params.id === 'string') {
        return parseInt(route.params.id, 10)
    }
    return 0;
});

const loadSite = async (idString: string | string[]) => {
    if (typeof idString === 'string') {
        const id = parseInt(idString, 10)
        await siteStore.load(id)
        await mapStore.loadWorldSiteMap(id, 'Default')
    }
}

loadSite(route.params.id)

const lists = computed(() => [
    { title: 'Structures', links: siteStore.site?.structuresLinks, icon: "mdi-domain", subtitle: "Notable buildings at this site" },
    { title: 'Related Historical Figures', links: siteStore.site?.relatedHistoricalFigureLinks, icon: "mdi-account-star", subtitle: "Famous figures tied to this site" },
    { title: 'Notable Deaths', links: siteStore.site?.notableDeathLinks, icon: "mdi-grave-stone", subtitle: "Key figures who died here" },
    { title: 'Battles', links: siteStore.site?.battleLinks, icon: "mdi-chess-bishop", subtitle: "Battles fought at this site" },
    { title: 'Conquerings', links: siteStore.site?.conqueringLinks, icon: "mdi-chess-pawn", subtitle: "Conquests that took place here" },
    { title: 'Raids', links: siteStore.site?.raidLinks, icon: "mdi-lightning-bolt", subtitle: "Raids carried out at this site" },
    { title: 'Duels', links: siteStore.site?.duelLinks, icon: "mdi-sword-cross", subtitle: "Duels fought at this site" },
    { title: 'Persecutions', links: siteStore.site?.persecutionLinks, icon: "mdi-map-marker-alert", subtitle: "Acts of persecution at this site" },
    { title: 'Insurrections', links: siteStore.site?.insurrectionLinks, icon: "mdi-map-marker-alert", subtitle: "Revolts and uprisings here" },
    { title: 'Abductions', links: siteStore.site?.abductionLinks, icon: "mdi-map-marker-alert", subtitle: "Kidnappings that happened here" },
    { title: 'Beast Attacks', links: siteStore.site?.beastAttackLinks, icon: "mdi-chess-knight", subtitle: "Beast attacks recorded here" }
]);

watch(
    () => route.params.id,
    loadSite
)

</script>

<template>
    <v-fab
        class="me-2"
        icon="mdi-chevron-right"
        location="top end"
        absolute
        :to="'/site/' + (routeId + 1)">
    </v-fab>
    <v-fab
        class="me-16"
        icon="mdi-chevron-left"
        location="top end"
        absolute
        :to="'/site/' + (routeId - 1)">
    </v-fab>
    <v-row>
        <v-col cols="12" xl="4" lg="6" md="12">
            <v-card
                :title="siteStore.site?.name ?? ''"
                :subtitle="siteStore.site?.type ?? ''"
                max-height="400"
                variant="text">
                <template v-slot:prepend>
                    <div class="large-icon" v-html="siteStore.site?.icon"></div>
                </template>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="mapStore.currentWorldSiteMap" cols="12" xl="4" lg="6" md="12">
            <v-card 
                prepend-icon="mdi-map-search-outline"
                title="Location"
                :subtitle="'The location of ' + siteStore.site?.name + ' on the world map'"
                height="400" variant="text" to="/map">
                <v-card-text>
                    <v-img width="320" height="320" class="position-relative ml-8" :src="mapStore.currentWorldSiteMap" :cover="false"/>
                </v-card-text>
        </v-card>
        </v-col>
        <v-col cols="12" xl="4" lg="6" md="12">
            <v-card
                max-height="400"
                variant="text">
                <v-card-text>
                    <v-list lines="two" class="mt-16">
                        <v-list-item v-if="siteStore.site?.regionToLink"
                            title="Region">
                            <v-list-item-subtitle>
                                <div v-html="siteStore.site?.regionToLink"></div>
                            </v-list-item-subtitle>
                        </v-list-item>
                        <v-list-item v-if="siteStore.site?.currentOwnerToLink"
                            title="Current Owner">
                            <v-list-item-subtitle>
                                <div v-html="siteStore.site?.currentOwnerToLink"></div>
                            </v-list-item-subtitle>
                        </v-list-item>
                    </v-list>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <template v-for="(list, i) in lists" :key="i">
            <v-col v-if="list?.links?.length" cols="12" xl="4" lg="6" md="12">
                <v-card
                    :prepend-icon="list.icon"
                    :title="list.title"
                    :subtitle="list.subtitle"
                    height="400"
                    variant="text">
                    <v-card-text>
                        <v-list height="300" scrollable>
                            <v-list-item  v-for="(link, i) in list?.links" :key="i">
                                <v-list-item-title v-html="link"/>
                            </v-list-item>
                        </v-list>
                    </v-card-text> 
                </v-card>
            </v-col>
        </template>
    </v-row>
</template>

<style>
.large-icon {
    font-size: 32px;
}
</style>