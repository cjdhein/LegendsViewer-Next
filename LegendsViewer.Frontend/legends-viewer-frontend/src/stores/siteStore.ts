import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema
import { LoadItemsSortOption } from '../types/legends';

export type Site = components['schemas']['Site'];
export type WorldEventDto = components['schemas']['WorldEventDto'];

export const useSiteStore = defineStore('site', {
    state: () => ({
        object: null as Site | null,
        objectEvents: [] as WorldEventDto[],
        objectEventsTotalItems: 0 as number,
        objectEventsPerPage: 10 as number,
        isLoading: false as boolean
    }),
    actions: {
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
    },
})