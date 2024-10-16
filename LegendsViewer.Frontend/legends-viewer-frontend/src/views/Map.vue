<template>
    <div class="map-container">
      <div id="map" style="height: 100%;"></div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, watch, ref, onBeforeUnmount } from 'vue';
import { useWorldStore } from '../stores/worldStore';
import { useWorldMapStore } from '../stores/mapStore';
import L, { Map, ImageOverlay, Layer } from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type SiteType = components['schemas']['SiteType'];

interface MarkerConfig {
  shape: 'circle' | 'triangle' | 'square' | 'pentagon' | 'hexagon' | 'star';
  size?: number;
}

const siteTypeMarkers: Record<SiteType, MarkerConfig> = {
  Unknown: { shape: 'circle' },

  // Dwarves
  Hillocks: { shape: 'square', size: 2 },
  Fortress: { shape: 'pentagon' },
  MountainHalls: { shape: 'hexagon', size: 4 },

  // Elves
  ForestRetreat: { shape: 'pentagon' },

  // Human
  Hamlet: { shape: 'square', size: 2 },
  Town: { shape: 'pentagon' },
  Castle: { shape: 'hexagon', size: 4 },

  // Goblins
  DarkPits: { shape: 'pentagon' },
  DarkFortress: { shape: 'hexagon' },

  // Main Civilizations
  Monastery: { shape: 'triangle' },
  Fort: { shape: 'triangle' },
  Tomb: { shape: 'triangle' },

  // Nature (Kobolds often start there)
  Cave: { shape: 'circle' },

  // Monsters
  Lair: { shape: 'circle', size: 2 },

  // Demons
  Vault: { shape: 'star' },

  // Minotaur
  Labyrinth: { shape: 'star' },

  // Titan and Colossus
  Shrine: { shape: 'star' },

  // Necromancer
  Tower: { shape: 'star', size: 4 },

  // Others
  Camp: { shape: 'circle' },
  ImportantLocation: { shape: 'star' },
};

function createMarker(siteType: SiteType, siteColor: string | null | undefined, latlng: L.LatLngExpression): L.Layer {
  const config = siteTypeMarkers[siteType];
  const color = siteColor || "#888888"
  const size = config.size || 3;
  switch (config.shape) {
    case 'circle':
      return L.circle(latlng, { color: color, radius: size });
    case 'triangle':
      return createPolygon(latlng, 3, size, color);
    case 'square':
      return createPolygon(latlng, 4, size, color);
    case 'pentagon':
      return createPolygon(latlng, 5, size, color);
    case 'hexagon':
      return createPolygon(latlng, 6, size, color);
    case 'star':
      return createStar(latlng, 5, size, size / 2, color);
    default:
      return L.circle(latlng, { color: color, radius: size / 2 });
  }
}

function createPolygon(center: L.LatLngExpression, sides: number, size: number, color: string): L.Polygon {
  const vertices: L.LatLngExpression[] = [];
  for (let i = 0; i < sides; i++) {
    const angle = (i / sides) * 2 * Math.PI + Math.PI / 2; // Add 90 degrees (π/2 radians)
    const vertex: L.LatLngExpression = [
      (center as number[])[0] + size * Math.sin(angle),
      (center as number[])[1] + size * Math.cos(angle)
    ]
    vertices.push(vertex);
  }
  return L.polygon(vertices, { color });
}

function createStar(center: L.LatLngExpression, points: number, outer: number, inner: number, color: string): L.Polygon {
  const vertices: L.LatLngExpression[] = [];
  for (let i = 0; i < points * 2; i++) {
    const angle = (i / (points * 2)) * 2 * Math.PI + Math.PI / 2; // Add 90 degrees (π/2 radians)
    const radius = i % 2 === 0 ? outer : inner;
    const vertex: L.LatLngExpression = [
      (center as number[])[0] + radius * Math.sin(angle),
      (center as number[])[1] + radius * Math.cos(angle)
    ]
    vertices.push(vertex);
  }
  return L.polygon(vertices, { color });
}

export default defineComponent({
  setup() {
    const worldStore = useWorldStore();
    const mapStore = useWorldMapStore();
    const leafletMap = ref<Map>();
    const currentOverlay = ref<ImageOverlay | null>(null);

    const initMap = async () => {
      if (!leafletMap.value) {
        leafletMap.value = L.map('map', {
          crs: L.CRS.Simple,
          zoom: 0,
          minZoom: -2,
          maxZoom: 2
        });
      }
      await worldStore.loadWorld();
      await mapStore.loadWorldMap('Large');
      if (mapStore.worldMapMax != null) {
        loadImageToMap(mapStore.worldMapMax);
      }
    };

    const loadImageToMap = (base64Image: string) => {
      if (!leafletMap.value) return;

      if (currentOverlay.value) {
        leafletMap.value.removeLayer(currentOverlay.value as unknown as Layer);
      }
      const scale = 8;
      const width = (worldStore.world.width ?? 0) + 1;
      const height = (worldStore.world.width ?? 0) + 1;

      const bounds: L.LatLngBoundsExpression = [[0, 0], [scale * height, scale * width]];
      const imageOverlay = L.imageOverlay(base64Image, bounds);

      imageOverlay.addTo(leafletMap.value);
      currentOverlay.value = imageOverlay;

      if (worldStore.world.siteMarkers != null) {
        for (const siteMarker of worldStore.world.siteMarkers) {
          if (siteMarker.coordinates != null) {
            for (const coordinate of siteMarker.coordinates) {
              if (coordinate.x != null && coordinate.y != null) {
                createMarker(siteMarker.type ?? 'Unknown', siteMarker.color, [(height - coordinate.y) * scale - 0.5 * scale, coordinate.x * scale + 0.5 * scale])
                 .addTo(leafletMap.value)
                 .bindPopup(`<a href="./site/${siteMarker.id}"><b>${siteMarker.name}</b></a><br>${siteMarker.type}`);;
              }
            }
          }
        }
      }

      leafletMap.value.fitBounds(bounds);
    };

    watch(() => mapStore.worldMapMax, (newBase64Map) => {
      if (newBase64Map) {
        loadImageToMap(newBase64Map);
      }
    });

    onMounted(() => {
      initMap();
    });

    onBeforeUnmount(() => {
      if (leafletMap.value) {
        leafletMap.value.remove();
      }
    });

    return {
      mapStore,
    };
  },
});
</script>

<style>
.map-container {
  height: 880px;
  width: 100%;
}

.leaflet-container {
  background: rgb(var(--v-theme-background))
}

.leaflet-layer,
.leaflet-control-zoom-in,
.leaflet-control-zoom-out,
.leaflet-control-attribution ,
.leaflet-popup {
  filter: invert(100%) hue-rotate(180deg) brightness(95%) contrast(90%);
}
</style>