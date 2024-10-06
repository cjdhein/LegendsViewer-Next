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
    // Add more mappings for other resources like Artifact, etc.
};

// Create a store for 'Site'
export const useSiteStore = createWorldObjectStore<components['schemas']['Site']>('Site');

// Create a store for 'Region'
export const useRegionStore = createWorldObjectStore<components['schemas']['WorldRegion']>('Region');


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
