import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type WorldDto = components['schemas']['WorldDto'];

export const useWorldStore = defineStore('world', {
    state: () => ({
        world: '' as WorldDto,
        isLoading: false as boolean
    }),
    actions: {
        async loadWorld() {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/World");

            if (error !== undefined) {
                this.isLoading = false;
                console.error(error);
            } else if (data) {
                this.world = data;
                this.isLoading = false;
            }
        },
    },
})