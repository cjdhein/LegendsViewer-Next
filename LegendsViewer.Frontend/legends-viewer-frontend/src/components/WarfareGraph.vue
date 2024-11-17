<template>
    <div v-if="fullscreen == false" ref="cy" style="width: 100%; height: 360px;"></div>
    <div v-else ref="cy" style="width: 100%; height: 780px;"></div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import cytoscape from 'cytoscape';
import cola, { ColaLayoutOptions } from 'cytoscape-cola';
import tippy from 'tippy.js';
import 'tippy.js/dist/tippy.css';

cytoscape.use(cola);

const layoutOptions: ColaLayoutOptions = {
    name: 'cola',
    animate: false,
    nodeDimensionsIncludeLabels: true,
    avoidOverlap: true,
    padding: 50,
    centerGraph: true,
    fit: true,
    convergenceThreshold: 0.01,
    edgeLength: () => 100,
    nodeSpacing: (node) => (node.isOrphan() ? 50 : 10),
};

export default defineComponent({
    name: 'WarfareGraph',
    props: {
        data: {
            type: Object,
            required: true,
        },
        fullscreen: {
            type: Boolean,
            default: false
        }
    },
    setup(props) {
        const cy = ref<HTMLDivElement | null>(null);

        onMounted(() => {
            if (cy.value) {
                const cyInstance = cytoscape({
                    container: cy.value,
                    zoom: .5,
                    wheelSensitivity: 0.1,
                    elements: {
                        nodes: props.data.nodes,
                        edges: props.data.edges
                    },
                    style: [
                        {
                            selector: 'node',
                            style: {
                                'shape': 'roundrectangle',
                                'width': 'label',
                                'height': 'label',
                                'label': 'data(label)',
                                'text-wrap': 'wrap',
                                'font-family': 'Helvetica',
                                'font-size': 12,
                                'text-valign': 'center',
                                'background-color': 'data(backgroundColor)',
                                'background-opacity': 1,
                                'color': 'data(foregroundColor)',
                                "padding-top": '6px'
                            },
                        },
                        {
                            selector: ':parent',
                            style: {
                                'shape': 'round-hexagon',
                                'label': 'data(label)',
                                'text-background-shape': 'roundrectangle',
                                'text-background-padding': '4',
                                'text-background-color': 'data(backgroundColor)',
                                'text-background-opacity': 0.8,
                                'background-opacity': 0.8,
                                'text-valign': 'top',
                                'text-halign': 'center',
                                'text-margin-y': -6,
                                'font-size': 8
                            },
                        },
                        {
                            selector: 'node.is-civilization',
                            style: {
                                'padding-left': '16px',
                                'shape': 'round-hexagon',
                            },
                        },
                        {
                            selector: 'node.current',
                            style: {
                                'background-blacken': 0.1,
                                'border-width': '2px',
                                'border-style': 'dashed',
                                'border-color': 'orange'
                            },
                        },
                        {
                            selector: 'edge',
                            style: {
                                'label': 'data(label)',
                                'width': 'data(width)',
                                'line-color': 'data(backgroundColor)',
                                'line-cap': 'round',
                                'line-opacity': 0.8,
                                'curve-style': 'bezier',
                                'target-arrow-shape': 'triangle',
                                'target-arrow-color': 'data(backgroundColor)',
                                'text-rotation': 'autorotate',
                                'text-background-shape': 'roundrectangle',
                                'text-background-padding': '4',
                                'text-background-color': 'data(backgroundColor)',
                                'text-background-opacity': 1,
                                'font-size': 10,
                                'color': 'data(foregroundColor)',
                            },
                        },
                    ],
                    layout: layoutOptions,
                });

                cyInstance.center();

                cyInstance.on('tap', 'node', (evt) => {
                    const node = evt.target;
                    const href = node.data('href');

                    if (href) {
                        window.location.href = href;
                    }
                });
                cyInstance.on('tap', 'edge', (evt) => {
                    const edge = evt.target;
                    const href = edge.data('href');

                    if (href) {
                        window.location.href = href;
                    }
                });

                // Function to create a tooltip for an edge
                function createEdgeTooltip(edge: cytoscape.EdgeSingular) {
                    return tippy(document.createElement('div'), {
                        allowHTML: true,
                        content: edge.data('tooltip'),
                        trigger: 'manual',
                        placement: 'top',
                    });
                }

                // Add cursor change on hover
                cyInstance.on('mouseover', 'node, edge', () => {
                    cyInstance.container()!.style.cursor = 'pointer';
                });

                cyInstance.on('mouseout', 'node, edge', () => {
                    cyInstance.container()!.style.cursor = '';
                });
                
                // Add tooltips to edges
                cyInstance.edges().forEach((edge) => {
                    const tooltip = createEdgeTooltip(edge);

                    edge.on('mouseover', () => {
                        const bbox = edge.renderedBoundingBox();
                        const x = (bbox.x1 + bbox.x2) / 2;
                        const y = (bbox.y1 + bbox.y2) / 2;

                        // Get the offset of the Cytoscape container
                        const container = cyInstance.container();
                        const containerRect = container!.getBoundingClientRect();

                        // Adjust for container position
                        tooltip.setProps({
                            getReferenceClientRect: () => new DOMRect(
                                containerRect.left + x,
                                containerRect.top + y,
                                0,
                                0
                            ),
                        });

                        tooltip.show();
                    });
                    edge.on('mouseout', () => {
                        tooltip.hide();
                    });
                });
            }
        });

        return {
            cy,
        };
    },
});
</script>

<style scoped>
/* Additional styles can be added here */
</style>