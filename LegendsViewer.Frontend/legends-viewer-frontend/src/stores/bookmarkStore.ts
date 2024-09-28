import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type Bookmark = components['schemas']['Bookmark'];

export const useBookmarkStore = defineStore('bookmark', {
  state: () => ({
    bookmarks: [] as Bookmark[],
    loadingNewWorld: false as boolean
  }),
  actions: {
    async load(filePath: string) {
      // Set the state of the bookmark to 'Loading' if it exists
      let existingBookmark = this.bookmarks.find(bookmark => bookmark.filePath === filePath);
      if (existingBookmark) {
        existingBookmark.state = 'Loading';
      }
      else {
        this.loadingNewWorld = true;
      }

      const { data, error } = await client.POST("/api/Bookmark/load", {
        body: filePath, // Send the filePath as raw text
      });

      if (error !== undefined) {
        console.error(error);
      } else if (data) {
        const newBookmark = data as Bookmark;

        // Check if the bookmark already exists
        const index = this.bookmarks.findIndex(bookmark => bookmark.filePath === filePath);

        if (index !== -1) {
          // Update the existing bookmark
          this.bookmarks[index] = newBookmark;
        } else {
          // Add the new bookmark to the array
          this.bookmarks.push(newBookmark);
        }

        this.loadingNewWorld = false;
      }
    },
    async getAll() {
      const { data, error } = await client.GET("/api/Bookmark");
      if (error !== undefined) {
        console.error(error)
      } else {
        this.bookmarks = data as Bookmark[]
      }
    },
  },
})