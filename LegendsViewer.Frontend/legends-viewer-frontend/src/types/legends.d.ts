
export type LegendLinkListData = {
    title: string,
    subtitle: string,
    icon: string,
    items: string[]
}

export interface LoadItemsSortOption {
    key: string,
    order: "asc" | "desc"
}

export interface LoadItemsOptions {
    page: number;
    itemsPerPage: number;
    sortBy: LoadItemsSortOption[];
}
