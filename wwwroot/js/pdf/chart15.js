var canvas15 = document.getElementById('myChart15');
var ctx15 = canvas15.getContext('2d');
var datasets15 = [];
const colorsChart15 = generatePalette(10, 0.8);

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart15",
    success: function (data) {
        data.forEach(item => {
            const chartElement = document.getElementById(`progress-${item.idCharacterization}`);
            if (chartElement) {
                chartElement.hidden = false;

                updateProgress(chartElement, item.value1);
            }
        });


        const labels15 = data.map(item => item.label);

        const datasets15 = [{
            label: 'Valor',
            data: data.map(item => item.value1), // Asociar cada valor con su correspondiente label
            backgroundColor: colorsChart15 // Usar el color correspondiente según la posición
        }];


        const data15 = {
            labels: labels15,
            datasets: datasets15
        };


        const config15 = {
            type: 'bar',
            data: data15,
            options: {
                indexAxis: 'y',          
               
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    x: {
                        beginAtZero: true,
                        grid: {
                            color: function (context) {
                                if (context.tick.value === 70) {
                                    return '#ff0000';  // Color de la línea vertical
                                }
                                return '#e0e0e0'; // Color de las otras líneas de la cuadrícula
                            }
                        },
                        suggestedMax: 100,
                        title: {
                            display: true,
                            text: '% Promedio'
                        }
                    },
                    y: {
                        beginAtZero: true,                      
                    },
                   
                },
                plugins: {  
                    legend:false,
                    datalabels: {
                        color: 'black', // Cambia el color de los datalabels

                    },
                    annotation: {
                        annotations: {
                            line1: {
                                type: 'line',
                                xMin: 70,
                                xMax: 70,
                                borderColor: 'red',
                                borderWidth: 2,
                                label: {
                                    content: 'Patolog\u00EDa Existente',
                                    enabled: true,
                                    rotation: 90, //
                                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                                    color: 'white',
                                    font: {
                                        size: 12,
                                        weight: 'bold'
                                    },
                                    xAdjust:-20,
                                }
                            }
                        }
                    }
                }
            }
        };

        new Chart(ctx15, config15);  
        renderDimensionAccordion(data, 'effectsAndRecommendationsAlert15', 'value1', false);
     
    }
});




