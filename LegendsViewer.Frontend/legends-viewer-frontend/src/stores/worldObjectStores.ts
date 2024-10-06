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
    // Add more mappings for other resources like Artifact, etc.
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
