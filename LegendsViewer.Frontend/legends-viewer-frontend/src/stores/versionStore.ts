import { defineStore } from 'pinia';
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema';

export type VersionDto = components['schemas']['VersionDto'];

export const useVersionStore = defineStore('version', {
  state: () => ({
    version: '',
    loading: false as boolean,
  }),
  
  actions: {
    async loadVersion() {
      // Set loading state to true
      this.loading = true;

      try {
        // Fetch directory info from the backend
        const { data, error } = await client.GET('/api/version');

        if (error) {
          console.error(error);
        } else if (data) {
          // Set the received data to the store state
          this.version = (data as VersionDto).version ?? '-'
        }
      } catch (err) {
        console.error('Error loading version:', err);
      } finally {
        // Set loading state to false after the operation
        this.loading = false;
      }
    },
  },
});
