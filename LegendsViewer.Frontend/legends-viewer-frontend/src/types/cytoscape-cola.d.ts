declare module 'cytoscape-cola' {
    import { BaseLayoutOptions } from 'cytoscape';
  
    export interface ColaLayoutOptions extends BaseLayoutOptions {
      animate?: boolean;
      refresh?: number;
      maxSimulationTime?: number;
      ungrabifyWhileSimulating?: boolean;
      fit?: boolean;
      padding?: number;
      boundingBox?: { x1: number; y1: number; x2: number; y2: number } | { x1: number; y1: number; w: number; h: number };
      nodeDimensionsIncludeLabels?: boolean;
  
      // Callbacks
      ready?: () => void;
      stop?: () => void;
  
      // Positioning options
      randomize?: boolean;
      avoidOverlap?: boolean;
      handleDisconnected?: boolean;
      convergenceThreshold?: number;
      nodeSpacing?: (node: any) => number | number;
      flow?: { axis: 'x' | 'y'; minSeparation: number };
      alignment?: {
        vertical?: Array<Array<{ node: any; offset?: number }>>;
        horizontal?: Array<Array<{ node: any; offset?: number }>>;
      };
      gapInequalities?: Array<{ axis: 'x' | 'y'; left: any; right: any; gap: number }>;
      centerGraph?: boolean;
  
      // Edge length options
      edgeLength?: (edge: any) => number | number;
      edgeSymDiffLength?: (edge: any) => number | number;
      edgeJaccardLength?: (edge: any) => number | number;
  
      // Cola algorithm iterations
      unconstrIter?: number;
      userConstIter?: number;
      allConstIter?: number;
    }
  
    const cytoscapeCola: (cy: any) => void;
    export default cytoscapeCola;
  }
  