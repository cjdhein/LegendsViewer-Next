import { defineStore } from 'pinia'
import client from "../apiClient"; // Import the global client
import { components } from '../generated/api-schema'; // Import from the OpenAPI schema

export type Bookmark = components['schemas']['Bookmark'];

export const useBookmarkStore = defineStore('bookmark', {
    state: () => ({
        bookmarks: [] as Bookmark[],
        loading:  false
    }),
    actions: {
      async load(filePath: string) {
        for (const bookmark of this.bookmarks) {
          if (bookmark.filePath === filePath) {
            bookmark.state === 'Loading'
          }
        }
        const { data, error } = await client.POST("/api/WorldParser/parse", {
          filePath: filePath,
        });
        if (error !== undefined) {
          console.error(error)
        } else {
          console.log(data)
        }
      },
      async getAll() {
        const { data, error } = await client.GET("/api/WorldParser/bookmarks");
        if (error !== undefined) {
          console.error(error)
        } else {
          this.bookmarks = data as Bookmark[]
          console.log(this.bookmarks)
        }
      },
    },
  })