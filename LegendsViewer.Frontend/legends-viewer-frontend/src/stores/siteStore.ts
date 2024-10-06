import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema
import { LoadItemsSortOption } from '../types/legends';

type Site = components['schemas']['Site'];

type WorldObjectDto = components['schemas']['WorldObjectDto'];
type WorldEventDto = components['schemas']['WorldEventDto'];
type ChartDataDto = components['schemas']['ChartDataDto'];

export const useSiteStore = defineStore('site', {
    state: () => ({
        objects: [] as WorldObjectDto[],
        objectsTotalItems: 0 as number,
        objectsTotalFilteredItems: 0 as number,
        objectsPerPage: 10 as number,

        object: null as Site | null,
        objectEvents: [] as WorldEventDto[],
        objectEventsTotalItems: 0 as number,
        objectEventsPerPage: 10 as number,

        objectEventChartData: null as ChartDataDto | null,
        
        isLoading: false as boolean
    }),
    actions: {
        async loadOverview(pageNumber: number, pageSize: number, sortBy: LoadItemsSortOption[], search: string) {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/Site", {
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
                this.objects = data.items ?? [];
                this.objectsTotalItems = data.totalCount ?? 0;
                this.objectsTotalFilteredItems = data.totalFilteredCount ?? 0;
                this.isLoading = false;
            }
        },
        async load(id: number) {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/Site/{id}", {
                params: {
                    path: { id: id },
                },
            });

            if (error !== undefined) {
                this.isLoading = false;
                console.error(error);
            } else if (data) {
                this.object = data;
                this.isLoading = false;
            }
        },
        async loadEvents(id: number, pageNumber: number, pageSize: number, sortBy: LoadItemsSortOption[]) {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/Site/{id}/events", {
                params: {
                    path: {
                        id: id
                    },
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
                this.objectEvents = data.items ?? [];
                this.objectEventsTotalItems = data.totalCount ?? 0;
                this.isLoading = false;
            }
        },
        async loadEventChartData(id: number) {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/Site/{id}/eventchart", {
                params: {
                    path: {
                        id: id
                    },
                },
            });

            if (error !== undefined) {
                this.isLoading = false;
                console.error(error);
            } else if (data) {
                this.objectEventChartData = data;
                this.isLoading = false;
            }
        },
    },
})