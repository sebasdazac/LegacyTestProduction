var progressBar8_1 = document.getElementById('progressBar8-1');
var progressBar8_2 = document.getElementById('progressBar8-2');
var canvas8 = document.getElementById('myChart8');
var ctx8 = canvas8.getContext('2d');
var datasets8 = [];
const colorsChart8 = generatePalette(14, 0.8);
var characterization8 = document.getElementById('characterization8');


$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart8",
    success: function (response) {
   
        const labels8 = response.dimensions.map(item => item.label);

        const datasets8 = [{
            label: 'Valor',
            data: response.dimensions.map(item => item.averageValue), // Asociar cada valor con su correspondiente label
            backgroundColor: colorsChart8 // Usar el color correspondiente según la posición
        }];

        const data8 = {
            labels: labels8,
            datasets: datasets8
        };

        const config8 = {
            type: 'bar',
            data: data8,
            options: {
                indexAxis: 'y', // Esto hace que las barras sean horizontales                 
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
                            text: '% Promedio Dimensiones'
                        }
                    },
                    y: {
                        beginAtZero: true,
                    }
                },
                plugins: {
                    legend: false,
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
                                    content: 'Dimenci\u00F3n Existente',
                                    enabled: true,
                                    rotation: 90, //
                                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                                    color: 'white',
                                    font: {
                                        size: 14,
                                    },
                                    xAdjust: 20,
                                }
                            }
                        }
                    }
                }
            }
        };
        new Chart(ctx8, config8);


        updateProgress(progressBar8_1, response.data.value1);
        updateProgress(progressBar8_2, response.data.value2);
        characterization8.innerText = response.data.characterization;

        renderAccordion(response.data, 'effectsAndRecommendationsAlert8');

    }
});