<template>
    <v-card :title="title" :subtitle="subtitle" height="640" variant="text">
        <template v-slot:prepend>
            <v-icon class="mr-2" :icon="icon" size="32px"></v-icon>
        </template>
        <v-card-text>
            <v-list v-if="list?.length > 0" class="ml-12" height="500" lines="three" scrollable>
                <template v-for="(item) in list">
                    <v-list-item>
                        <template v-if="item?.thumbnail" v-slot:prepend>
                            <v-img v-if="item.thumbnail && item.currentSitesCount != null && item.currentSitesCount > 0"
                                width="92" height="92" class="mr-4 mt-n2 pixelated-image" :src="`data:image/png;base64,${item.thumbnail}`"
                                :cover="false" />
                            <v-img v-if="item.thumbnail && item.currentSitesCount != null && item.currentSitesCount == 0"
                                width="92" height="92" class="mr-4 mt-n2 pixelated-image"
                                :src="`data:image/png;base64,${item.thumbnail}`" :cover="false"
                                gradient="to top right, rgba(25,32,72,.75), rgba(25,32,72,.80)" />
                        </template>
                        <template v-if="item?.link" v-slot:title>
                            <div class="mb-2" v-html="item.link"></div>
                        </template>
                        <template v-if="item?.race" v-slot:subtitle>
                            <div>
                                <template v-if="item?.currentLeader">
                                    <div class="ml-3" v-html="'<b>' + item.currentLeader.title + ':</b> ' + item.currentLeader.subtitle"></div>
                                </template>
                                <v-chip v-if="item.currentSitesCount" class="ma-2" color="primary" label>
                                    <v-icon icon="mdi-home" start></v-icon>
                                    {{ item.currentSitesCount }}
                                </v-chip>
                                <v-chip v-if="item.lostSitesCount" class="ma-2" color="pink" label>
                                    <v-icon icon="mdi-home-minus-outline" start></v-icon>
                                    {{ item.lostSitesCount }}
                                </v-chip>
                                <v-chip v-if="item.entityPopulationCount" class="ma-2" color="primary" label>
                                    <v-icon icon="mdi-account-circle-outline" start></v-icon>
                                    {{ item.entityPopulationCount }}
                                </v-chip>
                                <v-chip v-if="item.entityPopulationMemberCount" class="ma-2" color="cyan" label>
                                    <v-icon icon="mdi-account-supervisor-circle-outline" start></v-icon>
                                    {{ item.entityPopulationMemberCount }}
                                </v-chip>
                            </div>
                        </template>
                        <template v-slot:append>
                            <div>
                                <div v-html="item.race"></div>
                            </div>
                        </template>
                    </v-list-item>
                </template>
            </v-list>
        </v-card-text>
    </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

type MainCivilizationDto = components['schemas']['MainCivilizationDto'];

export default defineComponent({
    name: 'CivilizationsCardList',
    props: {
        icon: {
            type: String,
            required: true
        },
        title: {
            type: String,
            required: true
        },
        subtitle: {
            type: String,
            required: true
        },
        list: {
            type: Array as PropType<MainCivilizationDto[]>,
            required: true
        }
    }
});
</script>

<style scoped>
.append-container {
    display: flex;
    flex-direction: column;
}
</style>