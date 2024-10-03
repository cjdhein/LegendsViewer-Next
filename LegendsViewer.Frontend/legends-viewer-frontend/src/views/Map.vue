<template>
  <v-card>
    <div class="map-container">
      <div id="map" style="height: 100%;"></div>
    </div>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, onMounted, watch, ref, onBeforeUnmount } from 'vue';
import { useWorldStore } from '../stores/worldStore';
import { useMapStore } from '../stores/mapStore';
import L, { Map, ImageOverlay, Layer } from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type SiteType = components['schemas']['SiteType'];

interface MarkerConfig {
  color: string;
  shape: 'circle' | 'triangle' | 'square' | 'pentagon' | 'hexagon' | 'star';
  size?: number;
}

const siteTypeMarkers: Record<SiteType, MarkerConfig> = {
  Unknown: { color: '#808080', shape: 'circle' },
  Cave: { color: '#8B4513', shape: 'circle' },
  Fortress: { color: '#808080', shape: 'square' },
  ForestRetreat: { color: '#228B22', shape: 'triangle' },
  DarkFortress: { color: '#4B0082', shape: 'square' },
  Town: { color: '#FFD700', shape: 'pentagon' },
  Hamlet: { color: '#98FB98', shape: 'circle' },
  Vault: { color: '#C0C0C0', shape: 'hexagon' },
  DarkPits: { color: '#800000', shape: 'circle' },
  Hillocks: { color: '#9ACD32', shape: 'triangle' },
  Tomb: { color: '#696969', shape: 'square' },
  Tower: { color: '#4682B4', shape: 'triangle' },
  MountainHalls: { color: '#B8860B', shape: 'pentagon' },
  Camp: { color: '#8FBC8F', shape: 'triangle' },
  Lair: { color: '#8B0000', shape: 'circle' },
  Labyrinth: { color: '#9932CC', shape: 'pentagon' },
  Shrine: { color: '#FFB6C1', shape: 'star' },
  ImportantLocation: { color: '#FF4500', shape: 'star' },
  Fort: { color: '#A9A9A9', shape: 'square' },
  Monastery: { color: '#F4A460', shape: 'pentagon' },
  Castle: { color: '#4169E1', shape: 'hexagon' }
};

function createMarker(siteType: SiteType, latlng: L.LatLngExpression): L.Layer {
  const config = siteTypeMarkers[siteType];
  const size = config.size || 4;
  switch (config.shape) {
    case 'circle':
      return L.circle(latlng, { color: config.color, radius: size });
    case 'triangle':
      return createPolygon(latlng, 3, size, config.color);
    case 'square':
      return createPolygon(latlng, 4, size, config.color);
    case 'pentagon':
      return createPolygon(latlng, 5, size, config.color);
    case 'hexagon':
      return createPolygon(latlng, 6, size, config.color);
    case 'star':
      return createStar(latlng, 5, size, size / 2, config.color);
    default:
      return L.circle(latlng, { color: config.color, radius: size / 2 });
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
    const mapStore = useMapStore();
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
    };

    const loadImageToMap = (base64Image: string) => {
      if (!leafletMap.value) return;

      if (currentOverlay.value) {
        leafletMap.value.removeLayer(currentOverlay.value as unknown as Layer);
      }
      const scale = 10;
      const width = worldStore.world.width ?? 1;
      const height = worldStore.world.width ?? 1;

      const bounds: L.LatLngBoundsExpression = [[0, 0], [scale * height, scale * width]];
      const imageOverlay = L.imageOverlay(base64Image, bounds);

      imageOverlay.addTo(leafletMap.value);
      currentOverlay.value = imageOverlay;

      if (worldStore.world.siteMarkers != null) {
        for (const siteMarker of worldStore.world.siteMarkers) {
          if (siteMarker.coordinates != null) {
            for (const coordinate of siteMarker.coordinates) {
              if (coordinate.x != null && coordinate.y != null) {
                createMarker(siteMarker.type ?? 'Unknown', [(height - coordinate.y) * scale - 0.5, coordinate.x * scale + 2.5])
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