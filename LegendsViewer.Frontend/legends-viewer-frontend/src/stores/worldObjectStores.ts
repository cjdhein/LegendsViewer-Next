import { defineStore } from 'pinia';
import client from "../apiClient"; // Import the global client
import { paths } from '../generated/api-schema';
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema
import { LoadItemsSortOption } from '../types/legends';
import { UnwrapRef } from 'vue';

type PathsWithMethod<TPaths, TMethod extends string> = {
    [K in keyof TPaths]: TPaths[K] extends { [method in TMethod]: any } ? K : never;
  }[keyof TPaths];

// Common types
type WorldObjectDto = components['schemas']['WorldObjectDto'];
type WorldEventDto = components['schemas']['WorldEventDto'];
type ChartDataDto = components['schemas']['ChartDataDto'];

const apiPaths: Record<string, {
    overview: PathsWithMethod<paths, "get">;
    object: PathsWithMethod<paths, "get">;
    objectEvents: PathsWithMethod<paths, "get">;
    eventChart: PathsWithMethod<paths, "get">;
}> = {
    Site: {
        overview: "/api/Site"  as PathsWithMethod<paths, "get">,
        object: "/api/Site/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Site/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Site/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Region: {
        overview: "/api/Region" as PathsWithMethod<paths, "get">,
        object: "/api/Region/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Region/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Region/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    UndergroundRegion: {
        overview: "/api/UndergroundRegion" as PathsWithMethod<paths, "get">,
        object: "/api/UndergroundRegion/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/UndergroundRegion/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/UndergroundRegion/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Landmass: {
        overview: "/api/Landmass" as PathsWithMethod<paths, "get">,
        object: "/api/Landmass/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Landmass/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Landmass/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    River: {
        overview: "/api/River" as PathsWithMethod<paths, "get">,
        object: "/api/River/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/River/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/River/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Structure: {
        overview: "/api/Structure" as PathsWithMethod<paths, "get">,
        object: "/api/Structure/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Structure/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Structure/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Construction: {
        overview: "/api/Construction" as PathsWithMethod<paths, "get">,
        object: "/api/Construction/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Construction/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Construction/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    MountainPeak: {
        overview: "/api/MountainPeak" as PathsWithMethod<paths, "get">,
        object: "/api/MountainPeak/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/MountainPeak/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/MountainPeak/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    HistoricalFigure: {
        overview: "/api/HistoricalFigure" as PathsWithMethod<paths, "get">,
        object: "/api/HistoricalFigure/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/HistoricalFigure/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/HistoricalFigure/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Entity: {
        overview: "/api/Entity" as PathsWithMethod<paths, "get">,
        object: "/api/Entity/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Entity/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Entity/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    DanceForm: {
        overview: "/api/DanceForm" as PathsWithMethod<paths, "get">,
        object: "/api/DanceForm/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/DanceForm/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/DanceForm/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    MusicalForm: {
        overview: "/api/MusicalForm" as PathsWithMethod<paths, "get">,
        object: "/api/MusicalForm/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/MusicalForm/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/MusicalForm/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    PoeticForm: {
        overview: "/api/PoeticForm" as PathsWithMethod<paths, "get">,
        object: "/api/PoeticForm/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/PoeticForm/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/PoeticForm/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    WrittenContent: {
        overview: "/api/WrittenContent" as PathsWithMethod<paths, "get">,
        object: "/api/WrittenContent/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/WrittenContent/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/WrittenContent/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Artifact: {
        overview: "/api/Artifact" as PathsWithMethod<paths, "get">,
        object: "/api/Artifact/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Artifact/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Artifact/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    // EventCollections
    War: {
        overview: "/api/War" as PathsWithMethod<paths, "get">,
        object: "/api/War/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/War/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/War/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Battle: {
        overview: "/api/Battle" as PathsWithMethod<paths, "get">,
        object: "/api/Battle/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Battle/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Battle/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Duel: {
        overview: "/api/Duel" as PathsWithMethod<paths, "get">,
        object: "/api/Duel/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Duel/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Duel/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Insurrection: {
        overview: "/api/Insurrection" as PathsWithMethod<paths, "get">,
        object: "/api/Insurrection/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Insurrection/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Insurrection/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Persecution: {
        overview: "/api/Persecution" as PathsWithMethod<paths, "get">,
        object: "/api/Persecution/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Persecution/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Persecution/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Purge: {
        overview: "/api/Purge" as PathsWithMethod<paths, "get">,
        object: "/api/Purge/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Purge/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Purge/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    Raid: {
        overview: "/api/Raid" as PathsWithMethod<paths, "get">,
        object: "/api/Raid/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/Raid/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/Raid/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    SiteConquered: {
        overview: "/api/SiteConquered" as PathsWithMethod<paths, "get">,
        object: "/api/SiteConquered/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/SiteConquered/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/SiteConquered/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
    BeastAttack: {
        overview: "/api/BeastAttack" as PathsWithMethod<paths, "get">,
        object: "/api/BeastAttack/{id}" as PathsWithMethod<paths, "get">,
        objectEvents: "/api/BeastAttack/{id}/events" as PathsWithMethod<paths, "get">,
        eventChart: "/api/BeastAttack/{id}/eventchart" as PathsWithMethod<paths, "get">
    },
};

export const useSiteStore = createWorldObjectStore<components['schemas']['Site']>('Site');
export const useRegionStore = createWorldObjectStore<components['schemas']['WorldRegion']>('Region');
export const useUndergroundRegionStore = createWorldObjectStore<components['schemas']['UndergroundRegion']>('UndergroundRegion');
export const useLandmassStore = createWorldObjectStore<components['schemas']['Landmass']>('Landmass');
export const useRiverStore = createWorldObjectStore<components['schemas']['River']>('River');
export const useStructureStore = createWorldObjectStore<components['schemas']['Structure']>('Structure');
export const useConstructionStore = createWorldObjectStore<components['schemas']['WorldConstruction']>('Construction');
export const useMountainPeakStore = createWorldObjectStore<components['schemas']['MountainPeak']>('MountainPeak');
export const useHistoricalFigureStore = createWorldObjectStore<components['schemas']['HistoricalFigure']>('HistoricalFigure');
export const useEntityStore = createWorldObjectStore<components['schemas']['Entity']>('Entity');
export const useDanceFormStore = createWorldObjectStore<components['schemas']['DanceForm']>('DanceForm');
export const useMusicalFormStore = createWorldObjectStore<components['schemas']['MusicalForm']>('MusicalForm');
export const usePoeticFormStore = createWorldObjectStore<components['schemas']['PoeticForm']>('PoeticForm');
export const useWrittenContentStore = createWorldObjectStore<components['schemas']['WrittenContent']>('WrittenContent');
export const useArtifactStore = createWorldObjectStore<components['schemas']['Artifact']>('Artifact');

export const useWarStore = createWorldObjectStore<components['schemas']['War']>('War');
export const useBattleStore = createWorldObjectStore<components['schemas']['Battle']>('Battle');
export const useDuelStore = createWorldObjectStore<components['schemas']['Duel']>('Duel');
export const useInsurrectionStore = createWorldObjectStore<components['schemas']['Insurrection']>('Insurrection');
export const usePersecutionStore = createWorldObjectStore<components['schemas']['Persecution']>('Persecution');
export const usePurgeStore = createWorldObjectStore<components['schemas']['Purge']>('Purge');
export const useRaidStore = createWorldObjectStore<components['schemas']['Raid']>('Raid');
export const useSiteConqueredStore = createWorldObjectStore<components['schemas']['SiteConquered']>('SiteConquered');
export const useBeastAttackStore = createWorldObjectStore<components['schemas']['BeastAttack']>('BeastAttack');

export function createWorldObjectStore<T>(resourceName: string) {
    const pathsForResource = apiPaths[resourceName];
    if (!pathsForResource) {
        throw new Error(`Invalid resource name: ${resourceName}`);
    }
    return defineStore(resourceName, {
        state: () => ({
            objects: [] as WorldObjectDto[],
            objectsTotalItems: 0 as number,
            objectsTotalFilteredItems: 0 as number,
            objectsPerPage: 10 as number,

            // object is typed specifically for each store
            object: null as T | null,
            objectEvents: [] as WorldEventDto[],
            objectEventsTotalItems: 0 as number,
            objectEventsPerPage: 10 as number,

            objectEventChartData: null as ChartDataDto | null,

            isLoading: false as boolean
        }),
        actions: {
            async loadOverview(pageNumber: number, pageSize: number, sortBy: LoadItemsSortOption[], search: string) {
                this.isLoading = true;
                const { data, error } = await client.GET(pathsForResource.overview, {
                    params: {
                        query: {
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            sortKey: sortBy[0]?.key,
                            sortOrder: sortBy[0]?.order,
                            search: search
                        },
                    },
                });

                if (error !== undefined) {
                    this.isLoading = false;
                    console.error(error);
                } else if (data) {
                    this.objects = (data as any).items ?? [];
                    this.objectsTotalItems = (data as any).totalCount ?? 0;
                    this.objectsTotalFilteredItems = (data as any).totalFilteredCount ?? 0;
                    this.isLoading = false;
                }
            },
            async load(id: number) {
                this.isLoading = true;
                const { data, error } = await client.GET(pathsForResource.object, {
                    params: {
                        path: { id: id },
                    },
                });

                if (error !== undefined) {
                    this.isLoading = false;
                    console.error(error);
                } else if (data) {
                    this.object = data as UnwrapRef<T> | null;
                    this.isLoading = false;
                }
            },
            async loadEvents(id: number, pageNumber: number, pageSize: number, sortBy: LoadItemsSortOption[]) {
                this.isLoading = true;
                const { data, error } = await client.GET(pathsForResource.objectEvents, {
                    params: {
                        path: { id: id },
                        query: {
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            sortKey: sortBy[0]?.key,
                            sortOrder: sortBy[0]?.order
                        },
                    },
                });

                if (error !== undefined) {
                    this.isLoading = false;
                    console.error(error);
                } else if (data) {
                    this.objectEvents = (data as any).items as WorldEventDto[] ?? [];
                    this.objectEventsTotalItems = (data as any).totalCount as number ?? 0;
                    this.isLoading = false;
                }
            },
            async loadEventChartData(id: number) {
                this.isLoading = true;
                const { data, error } = await client.GET(pathsForResource.eventChart, {
                    params: { path: { id: id } },
                });

                if (error !== undefined) {
                    this.isLoading = false;
                    console.error(error);
                } else if (data) {
                    this.objectEventChartData = data as ChartDataDto;
                    this.isLoading = false;
                }
            },
        },
    });
}
