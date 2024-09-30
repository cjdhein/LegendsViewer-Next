import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type MapSize = components['schemas']['MapSize'];

export const useMapStore = defineStore('map', {
    state: () => ({
        worldMapMin: '' as string,
        worldMapMid: '' as string,
        worldMapMax: '' as string,
        currentWorldSiteMap: '' as string,
        isLoading: false as boolean
    }),
    actions: {
        async loadWorldMap(size: MapSize) {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/WorldMap/world/{size}", {
                params: {
                    path: {
                        size: size
                    }
                }
            });

            if (error !== undefined) {
                this.isLoading = false;
                console.error(error);
            } else if (data) {
                const imageData = `data:image/png;base64,${data}`
                switch (size) {
                    case 'Small':
                        this.worldMapMin = imageData;
                        break;
                    case 'Large':
                        this.worldMapMax = imageData;
                        break;
                    
                    default:
                        this.worldMapMid = imageData;
                        break;
                }
                this.isLoading = false;
            }
        },
        async loadWorldSiteMap(id:number, size: MapSize) {
            this.isLoading = true;
            const { data, error } = await client.GET("/api/WorldMap/site/{id}/{size}", {
                params: {
                    path: {
                        id: id,
                        size: size
                    }
                }
            });

            if (error !== undefined) {
                this.isLoading = false;
                console.error(error);
            } else if (data) {
                this.currentWorldSiteMap = `data:image/png;base64,${data}`
                this.isLoading = false;
            }
        },
    },
})