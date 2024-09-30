import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type Site = components['schemas']['Site'];

export const useSiteStore = defineStore('site', {
    state: () => ({
        site: null as Site | null,
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
                this.site = data;
                console.log(this.site)
                this.isLoading = false;
            }
        },
    },
})