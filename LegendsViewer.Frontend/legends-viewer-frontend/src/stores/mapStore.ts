import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { paths } from '../generated/api-schema';
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema


type PathsWithMethod<TPaths, TMethod extends string> = {
    [K in keyof TPaths]: TPaths[K] extends { [method in TMethod]: any } ? K : never;
}[keyof TPaths];

export type MapSize = components['schemas']['MapSize'];

const apiPaths: Record<string, PathsWithMethod<paths, "get">> = {
    Site: "/api/WorldMap/site/{id}/{size}",
    Region: "/api/WorldMap/region/{id}/{size}",
    UndergroundRegion: "/api/WorldMap/undergroundregion/{id}/{size}",
    Landmass: "/api/WorldMap/landmass/{id}/{size}",
    River: "/api/WorldMap/river/{id}/{size}",
    Construction: "/api/WorldMap/construction/{id}/{size}",
    MountainPeak: "/api/WorldMap/mountainpeak/{id}/{size}",
    Entity: "/api/WorldMap/entity/{id}/{size}",
    Artifact: "/api/WorldMap/artifact/{id}/{size}",
    Structure: "/api/WorldMap/structure/{id}/{size}",

    World: "/api/WorldMap/world/{size}"
};

export const useWorldMapStore = createWorldMapStore('World');

export const useSiteMapStore = createWorldMapStore('Site');
export const useRegionMapStore = createWorldMapStore('Region');
export const useUndergroundRegionMapStore = createWorldMapStore('UndergroundRegion');
export const useLandmassMapStore = createWorldMapStore('Landmass');
export const useRiverMapStore = createWorldMapStore('River');
export const useConstructionMapStore = createWorldMapStore('Construction');
export const useMountainPeakMapStore = createWorldMapStore('MountainPeak');
export const useEntityMapStore = createWorldMapStore('Entity');
export const useArtifactMapStore = createWorldMapStore('Artifact');
export const useStructureMapStore = createWorldMapStore('Structure');

export function createWorldMapStore(resourceName: string) {
    const pathsForResource = apiPaths[resourceName];
    if (!pathsForResource) {
        throw new Error(`Invalid resource name: ${resourceName}`);
    }
    return defineStore(resourceName + "Map", {
        state: () => ({
            worldMapMin: '' as string,
            worldMapMid: '' as string,
            worldMapMax: '' as string,
            currentWorldObjectMap: '' as string,
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
            async loadWorldObjectMap(id: number, size: MapSize) {
                this.isLoading = true;
                const { data, error } = await client.GET(pathsForResource, {
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
                    this.currentWorldObjectMap = `data:image/png;base64,${data}`
                    this.isLoading = false;
                }
            },
        },
    });
}
