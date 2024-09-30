<script setup lang="ts">
import { useSiteStore } from '../stores/siteStore';
import { useRoute } from 'vue-router'
import { computed, watch } from 'vue'
import { useMapStore } from '../stores/mapStore';

const route = useRoute()
const siteStore = useSiteStore()
const mapStore = useMapStore()

const loadSite = async (idString: string | string[]) => {
    if (typeof idString === 'string') {
        const id = parseInt(idString, 10)
        await siteStore.load(id)
        await mapStore.loadWorldSiteMap(id, 'Default')
    }
}

loadSite(route.params.id)

const lists = computed(() => [
        { title: 'Structures', links: siteStore.site?.structuresLinks },
        { title: 'Related Historical Figures', links: siteStore.site?.relatedHistoricalFigureLinks },
        { title: 'Notable Deaths', links: siteStore.site?.notableDeathLinks },
        { title: 'Battles', links: siteStore.site?.battleLinks },
        { title: 'Conquerings', links: siteStore.site?.conqueringLinks },
        { title: 'Beast Attacks', links: siteStore.site?.beastAttackLinks },
    ]);

watch(
    () => route.params.id,
    loadSite
)

</script>

<template>
    <v-row>
        <v-col cols="12" md="12">
            <v-card class="mx-auto" max-height="400" variant="text">
                <v-card-title>
                    {{ siteStore.site?.name }}
                </v-card-title>
                <v-card-subtitle>
                    {{ siteStore.site?.type }}
                </v-card-subtitle>
                <v-card-text>
                    <template v-if="siteStore.site?.regionToLink">
                        <p>Region:</p>
                        <span v-html="siteStore.site?.regionToLink"></span>
                    </template>
                    <template v-if="siteStore.site?.currentOwnerToLink">
                        <p>Current Owner:</p>
                        <span v-html="siteStore.site?.currentOwnerToLink"></span>
                    </template>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
    <v-row>
        <v-col v-if="mapStore.currentWorldSiteMap" cols="12" md="3">
            <v-card class="mx-auto" max-width="400" height="400">
                <v-card-text>
                    <v-img :src="mapStore.currentWorldSiteMap"/>
                </v-card-text>
            </v-card>
        </v-col>
        <template v-for="(list, i) in lists" :key="i">
            <v-col v-if="list?.links?.length" cols="12" md="3">
                <v-card class="mx-auto" max-width="400" height="400">
                    <v-card-title>
                        {{ list.title }}
                    </v-card-title>
                    <v-card-text>
                        <v-list height="320" scrollable>
                            <v-list-item  v-for="(link, i) in list?.links" :key="i" prepend-icon="mdi-circle-medium">
                                <v-list-item-title v-html="link"/>
                            </v-list-item>
                        </v-list>
                    </v-card-text>
                </v-card>
            </v-col>
        </template>
    </v-row>
</template>