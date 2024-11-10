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
    const innerCircleBackground = '#f0f0f0'; // Light color for inner circle

    const names = Array.from(new Set(data.flatMap(d => [d.source, d.target])));
    const index = new Map(names.map((name, i) => [name, i]));
    const matrix = Array.from(index, () => new Array(names.length).fill(0));
    const sourceColors = new Map(data.map(d => [d.source, d.sourceColor]));
    const targetColors = new Map(data.map(d => [d.target, d.targetColor]));

    for (const { source, target, value } of data) {
        matrix[index.get(source)!][index.get(target)!] += value;
    }

    const chord = d3.chordDirected()
        .padAngle(12 / innerRadius)
        .sortSubgroups(d3.descending)
        .sortChords(d3.descending);

    const arc = d3.arc()
        .innerRadius(innerRadius)
        .outerRadius(outerRadius);

    const ribbon = d3.ribbonArrow()
        .radius(innerRadius - 1)
        .padAngle(1 / innerRadius);

    const svg = d3.select(element)
        .append('svg')
        .attr('width', width)
        .attr('height', height)
        .attr('viewBox', [-width / 2, -height / 2, width, height] as any)
        .attr('style', `width: 100%; height: auto; font: 10px sans-serif; background-color: ${backgroundColor};`);

    // Add a light background circle for the inner area
    svg.append('circle')
        .attr('cx', 0)
        .attr('cy', 0)
        .attr('r', innerRadius)
        .attr('fill', innerCircleBackground);

    const chords = chord(matrix);
    // Map chords to ribbon-compatible data
    const ribbonData = chords.map(chord => ({
        source: { ...chord.source, radius: innerRadius - 0.5 },
        target: { ...chord.target, radius: innerRadius - 0.5 },
    }));
    // Define a circular path for the text
    const textPathId = "outerCirclePath";
    svg.append("defs")
        .append("path")
        .attr("id", textPathId)
        .attr("d", d3.arc()({
            innerRadius: 0,
            outerRadius: outerRadius + 10,
            startAngle: 0,
            endAngle: 2 * Math.PI
        }));

    // Add ribbons with clickable links
    svg.append('g')
        .attr('fill-opacity', 0.75)
        .selectAll('a')
        .data(ribbonData)
        .join('a')
        .attr('xlink:href', d => {
            const source = names[d.source.index];
            const target = names[d.target.index];
            return data.find(item => item.source === source && item.target === target)!.href ?? null;
        })
        .append('path')
        .attr('d', ribbon)
        .attr('fill', d => sourceColors.get(names[d.source.index]) || '#ccc')
        .style('mix-blend-mode', 'multiply')
        .append('title')
        .text(d => {
            const source = names[d.source.index];
            const target = names[d.target.index];
            return data.find(item => item.source === source && item.target === target)!.tooltip ?? null;
        });

    const g = svg.append('g')
        .selectAll('g')
        .data(chords.groups)
        .join('g');

    // Draw arcs with color matching the source and tooltips from data
    g.append('path')
        .attr('d', d => arc({
            ...d,
            innerRadius,
            outerRadius
        }))
        .attr('fill', d => sourceColors.get(names[d.index]) || targetColors.get(names[d.index]) || '#CCC')
        .attr('stroke', strokeColor)
        .append('title')
        .text(d => {
            const name = names[d.index];
            return data.find(item => item.source === name)?.tooltip || '';
        });

    // Add text labels to follow the outer circle path
    g.append("text")
        .attr("dy", -3)
        .append("textPath")
        .attr("xlink:href", `#${textPathId}`)
        .attr("startOffset", d => `${(d.startAngle / (2 * Math.PI)) * 100}%`)
        .attr("fill", textColor)
        .attr("font-size", fontSize + "px") // Adjust text size as needed
        .text(d => names[d.index] ?? "Unknown");
}

</script>

<style scoped>
/* Add any additional styling here */
</style>
