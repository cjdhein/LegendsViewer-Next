<template>
    <Doughnut :data="extendedChartData" :options="chartOptions" />
</template>

<script>
import { defineComponent, computed } from 'vue';
import { Doughnut } from 'vue-chartjs'
import { Chart as ChartJS, ArcElement, Title, Tooltip, Legend, Colors, Filler } from 'chart.js'

ChartJS.register(ArcElement, Title, Tooltip, Legend, Colors, Filler)

export default defineComponent({
    name: 'DoughnutChart',
    components: { Doughnut },
    props: {
        chartData: {
            type: Object,
            required: true
        },
        chartOptions: {
            type: Object,
            default: () => {
                return {
                    responsive: true,
                    maintainAspectRatio: false,
                }
            }
        },
    },
    setup(props) {
        // Using a computed property to extend chartData with default values
        const extendedChartData = computed(() => {
            const borderRadius = 4;
            const spacing = 4;
            if (!props.chartData) 
                return {
                    labels: ['VueJs', 'EmberJs', 'ReactJs', 'AngularJs'],
                    datasets: [{
                        borderColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'],
                        backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'],
                        data: [40, 20, 80, 10],
                        borderRadius: borderRadius,
                        spacing: spacing,
                    }]
                }

            // Add borderRadius and spacing if they don't exist
            return {
                ...props.chartData,
                datasets: props.chartData.datasets.map(dataset => ({
                    ...dataset,
                    borderRadius: borderRadius,
                    spacing: spacing,
                })),
            };
        });

        return {
            extendedChartData,
        };
    }
})

ChartJS.defaults.color = '#ffffff';  // Default text color
ChartJS.defaults.scale.ticks.color = '#b0bec5';  // Axis tick colors
ChartJS.defaults.scale.grid.color = '#37474f';  // Grid line colors
ChartJS.overrides['doughnut'].plugins.legend.position = 'right'
</script>