<template>
    <div ref="chartContainer"></div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import * as d3 from 'd3';

export default defineComponent({
    name: 'DirectedChordDiagram',
    props: {
        data: {
            type: Array as () => Array<{
                source?: string | null;
                target?: string | null;
                value?: number | undefined;
                sourceColor?: string | null;
                targetColor?: string | null;
                tooltip?: string | null;
                href?: string | null;
            }>,
            required: true,
        },
        fontSize: {
            type: Number,
            required: true
        }
    },
    setup(props) {
        const chartContainer = ref<HTMLDivElement | null>(null);

        onMounted(() => {
            if (chartContainer.value) {
                createChordDiagram(chartContainer.value, props.data, props.fontSize);
            }
        });

        return { chartContainer };
    },
});

function createChordDiagram(
    element: HTMLDivElement,
    data: Array<{
        source?: string | null;
        target?: string | null;
        value?: number | undefined;
        sourceColor?: string | null;
        targetColor?: string | null;
        tooltip?: string | null;
        href?: string | null;
    }>,
    fontSize: number
) {
    const width = 1080;
    const height = width;
    const innerRadius = Math.min(width, height) * 0.5 - 48;
    const outerRadius = innerRadius + 12;

    const backgroundColor = '#121212';
    const textColor = '#ffffff';
    const strokeColor = '#888888';
    const innerCircleBackground = '#f0f0f0';

    // Set up unique names and map each to an index for angle calculation
    const names = Array.from(new Set(data.flatMap(d => [d.source, d.target])));
    const index = new Map(names.map((name, i) => [name, i]));

    // Calculate total incoming and outgoing connections for each node
    const nodeConnections: Record<string, { incoming: number; outgoing: number }> = {};
    data.forEach(d => {
        if (d.source) {
            nodeConnections[d.source] = nodeConnections[d.source] || { incoming: 0, outgoing: 0 };
            nodeConnections[d.source].outgoing++;
        }
        if (d.target) {
            nodeConnections[d.target] = nodeConnections[d.target] || { incoming: 0, outgoing: 0 };
            nodeConnections[d.target].incoming++;
        }
    });

    // Define arc and ribbon generators
    const arc = d3.arc()
        .innerRadius(innerRadius)
        .outerRadius(outerRadius);

    const ribbon = d3.ribbonArrow()
        .radius(innerRadius - 1)
        .padAngle(0.02);  // Use a small padding for separation

    const svg = d3.select(element)
        .append('svg')
        .attr('width', width)
        .attr('height', height)
        .attr('viewBox', [-width / 2, -height / 2, width, height] as any)
        .attr('style', `width: 100%; height: auto; font: 10px sans-serif; background-color: ${backgroundColor};`);

    // Add a background circle for visual reference
    svg.append('circle')
        .attr('cx', 0)
        .attr('cy', 0)
        .attr('r', innerRadius)
        .attr('fill', innerCircleBackground);

    // Define a circular path for text labels
    const textPathId = "outerCirclePath";
    svg.append("defs")
        .append("path")
        .attr("id", textPathId)
        .attr("d", d3.arc()({
            innerRadius: 0,
            outerRadius: outerRadius + 10,
            startAngle: 0,
            endAngle: 2 * Math.PI
        }) as string);

    // Draw individual ribbons for each entry in data
    const ribbonGroup = svg.append('g')
        .attr('fill-opacity', 0.75);

    data.forEach((d, i) => {
        const sourceIndex = index.get(d.source) ?? 0;
        const targetIndex = index.get(d.target) ?? 0;

        // Determine the number of outgoing and incoming connections
        const sourceOutgoing = nodeConnections[d.source!].outgoing;
        const targetIncoming = nodeConnections[d.target!].incoming;

        // Allocate segment sizes dynamically based on the number of connections for each node
        const sourceSegmentSize = (2 * Math.PI) / (names.length * sourceOutgoing);
        const targetSegmentSize = (2 * Math.PI) / (names.length * targetIncoming);

        // Calculate specific start and end angles for each ribbon, using the dynamically calculated segment sizes
        const sourceSectorStart = sourceIndex * (2 * Math.PI / names.length);
        const targetSectorStart = targetIndex * (2 * Math.PI / names.length);

        const sourceStartAngle = sourceSectorStart + (i % sourceOutgoing) * sourceSegmentSize;
        const sourceEndAngle = sourceStartAngle + sourceSegmentSize - 0.01;  // Apply padding

        const targetStartAngle = targetSectorStart + (i % targetIncoming) * targetSegmentSize;
        const targetEndAngle = targetStartAngle + targetSegmentSize - 0.01;  // Apply padding

        // Draw each ribbon separately
        ribbonGroup
            .append('a')
            .attr('xlink:href', d.href ?? null)
            .attr('target', '_blank')
            .append('path')
            .attr('d', ribbon({
                source: { startAngle: sourceStartAngle, endAngle: sourceEndAngle, radius: innerRadius },
                target: { startAngle: targetStartAngle, endAngle: targetEndAngle, radius: innerRadius }
            }) as unknown as string)
            .attr('fill', d.sourceColor || '#ccc')
            .style('mix-blend-mode', 'multiply')
            .append('title')
            .text(d.tooltip ?? "");
    });

    // Draw arcs for each unique name in `names`
    const g = svg.append('g')
        .selectAll('g')
        .data(names)
        .join('g');

    g.append('path')
        .attr('d', d => arc({
            innerRadius,
            outerRadius,
            startAngle: (index.get(d)! / names.length) * 2 * Math.PI,
            endAngle: ((index.get(d)! + 1) / names.length) * 2 * Math.PI
        }) as string)
        .attr('fill', d => data.find(item => item.source === d)?.sourceColor || data.find(item => item.target === d)?.targetColor || '#CCC')
        .attr('stroke', strokeColor)
        .append('title')
        .text(d => d ?? "Unknown");

    // Add text labels around the outer circle
    g.append("text")
        .attr("dy", -3)
        .append("textPath")
        .attr("xlink:href", `#${textPathId}`)
        .attr("startOffset", (_, i) => `${(i / names.length) * 100}%`)
        .attr("fill", textColor)
        .attr("font-size", fontSize + "px")
        .text(d => d ?? "Unknown");
}

</script>

<style scoped>
/* Add any additional styling here */
</style>
