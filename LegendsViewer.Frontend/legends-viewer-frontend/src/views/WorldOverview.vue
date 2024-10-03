<script setup lang="ts">
import { ref } from 'vue';
import { useBookmarkStore } from '../stores/bookmarkStore';
import { useFileSystemStore } from '../stores/fileSystemStore';

const bookmarkStore = useBookmarkStore()
const fileSystemStore = useFileSystemStore()
bookmarkStore.getAll()
fileSystemStore.getRoot();

const fileName = ref<string>('')

// Function to prepare a proper base64 string for png images
const getImageData = (bookmark: any) => {
  if (!bookmark.worldMapImage) {
    return ''; // Return an empty string if there's no image data
  }
  return `data:image/png;base64,${bookmark.worldMapImage}`;
}

const readFromClipboard = async () => {
  try {
    // Read the text from the clipboard
    let clipboardText = await navigator.clipboard.readText();

    console.log('Clipboard Text:', clipboardText);

    if (clipboardText.includes("\"")) {
      clipboardText = clipboardText.replace("\"", "").replace("\"", "")
      console.log('Clipboard Text:', clipboardText);
    }

    // Regular expression to check if the path ends with .xml
    const xmlFileRegex = /^(.*[\\/])?([^\\/]+\.xml)$/i;

    // Check if the text from the clipboard is a valid path that ends with .xml
    const match = clipboardText.match(xmlFileRegex);

    if (match) {
      const fullPath = match[0]; // Full path
      const filename = match[2]; // Filename (the second capture group from the regex)

      console.log('Full path:', fullPath);
      console.log('Filename:', filename);

      fileSystemStore.loadDirectory(fullPath)
      fileName.value = filename
    } else {
      fileSystemStore.loadDirectory(clipboardText)
    }
  } catch (err) {
    console.error('Failed to read from clipboard:', err);
  }
}

</script>

<template>
  <v-row dense>
    <v-col v-for="(bookmark, i) in bookmarkStore.bookmarks" :key="i" cols="12" md="3">
      <v-card class="mx-auto" max-width="320">
        <v-container>
          <v-img height="300px" width="300px" :src="getImageData(bookmark)" cover>
          </v-img>
        </v-container>

        <v-card-title>
          {{bookmark.worldName}}
          <v-chip class="float-right">
            {{ bookmark.worldWidth + " x " + bookmark.worldHeight }}
          </v-chip>
        </v-card-title>

        <v-card-subtitle>
          {{ bookmark.worldAlternativeName }}
        </v-card-subtitle>

        <v-card-actions>
          <v-btn v-if="bookmark.filePath && bookmark.state !== 'Loaded' || bookmark.latestTimestamp !== bookmark.loadedTimestamp" :loading="bookmark.state === 'Loading'"
            color="blue" text="Load" :disabled="bookmarkStore.isLoading" @click="bookmarkStore.loadByFullPath(bookmark.filePath ?? '', bookmark.latestTimestamp ?? '')">
          </v-btn>
          <v-btn v-if="bookmark.filePath && bookmark.state === 'Loaded' && bookmark.latestTimestamp === bookmark.loadedTimestamp" color="green-lighten-2" text="Explore"
            :disabled="bookmarkStore.isLoading" to="/map">
          </v-btn>

          <v-spacer></v-spacer>
          <v-combobox
            v-model="bookmark.latestTimestamp"
            :items="bookmark.worldTimestamps ?? []"
            density="compact"
            label="Timestamps"
            width="160"
          ></v-combobox>
        </v-card-actions>
      </v-card>
    </v-col>
    <v-col>
      <v-card class="mx-auto" max-width="320">
        <v-container>
          <v-icon icon="mdi-earth-plus" size="300"></v-icon>
        </v-container>

        <v-card-title>
          Explore a new world
        </v-card-title>

        <v-card-subtitle>
          Select an exported legends XML file
        </v-card-subtitle>

        <v-card-actions>
          <v-dialog width="auto" min-width="480">
            <template v-slot:activator="{ props: activatorProps }">
              <v-btn color="orange-lighten-2" prepend-icon="mdi-earth" text="Select" v-bind="activatorProps" :disabled="bookmarkStore.isLoading"></v-btn>
            </template>

            <template v-slot:default="{ isActive }">
              <v-card prepend-icon="mdi-earth" title="Select World Export">
                <v-divider class="mt-3"></v-divider>

                <v-card-text class="px-4">
                  <v-form>
                    <v-text-field
                      v-model="fileSystemStore.filesAndSubdirectories.currentDirectory"
                      readonly
                      label="Current Folder">
                      <template v-slot:append>
                        <v-btn
                          aria-label="Copy path from clipboard"
                          icon="mdi-clipboard-outline"
                          @click="readFromClipboard()">
                        </v-btn>
                      </template> </v-text-field>
                    <v-text-field v-model="fileName" readonly label="File Name"></v-text-field>

                  </v-form>

                  <v-list density="compact" height="220" scrollable>
                    <v-list-subheader>Directories</v-list-subheader>
                    <v-list-item
                      v-if="fileSystemStore.filesAndSubdirectories.currentDirectory != '/'"
                      @click="fileSystemStore.loadDirectory(fileSystemStore.filesAndSubdirectories.parentDirectory ?? '/')"
                      color="primary" variant="plain">
                      <template v-slot:prepend>
                        <v-icon icon="mdi-folder-outline"></v-icon>
                      </template>

                      <v-list-item-title v-text="'..'"></v-list-item-title>
                    </v-list-item>

                    <v-list-item v-for="(item, i) in fileSystemStore.filesAndSubdirectories.subdirectories" :key="i"
                      :value="item"
                      @click="fileSystemStore.loadSubDirectory(fileSystemStore.filesAndSubdirectories.currentDirectory ?? '/', item); fileName = ''"
                      color="primary" variant="plain">
                      <template v-slot:prepend>
                        <v-icon icon="mdi-folder-outline"></v-icon>
                      </template>

                      <v-list-item-title v-text="item"></v-list-item-title>
                    </v-list-item>
                  </v-list>

                  <v-list density="compact" min-height="220">
                    <v-list-subheader>World Exports</v-list-subheader>
                    <v-list-item v-for="(item, i) in fileSystemStore.filesAndSubdirectories.files" :key="i"
                      :value="item" color="primary" variant="plain">
                      <template v-slot:prepend>
                        <v-icon icon="mdi-file-xml-box"></v-icon>
                      </template>

                      <v-list-item-title v-text="item"
                        @click="fileName = item">
                      </v-list-item-title>
                    </v-list-item>
                  </v-list>

                </v-card-text>

                <v-divider></v-divider>

                <v-card-actions>
                  <v-btn text="Close" @click="isActive.value = false"></v-btn>

                  <v-spacer></v-spacer>

                  <v-btn color="surface-variant" text="Load World" variant="flat"
                    :disabled="fileName == null || fileName == ''"
                    @click="bookmarkStore.loadByFolderAndFile(fileSystemStore.filesAndSubdirectories.currentDirectory ?? '/', fileName); isActive.value = false;"></v-btn>
                </v-card-actions>
              </v-card>
            </template>
          </v-dialog>
          <v-spacer></v-spacer>

        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>
