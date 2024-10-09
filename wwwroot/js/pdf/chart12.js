var canvas12 = document.getElementById('myChart12');
var ctx12 = canvas12.getContext('2d');
var datasets12 = [];
const colorsChart12 = generatePalette(10, 0.8);

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart12?param=" + param,
    success: function (data) {

        data.forEach(item => {
            const chartElement = document.getElementById(`progress-${item.idCharacterization}`);
            if (chartElement) {
                chartElement.hidden = false;

                updateProgress(chartElement, item.value1);
            }
        });

        const labels12 = data.map(item => item.label);

        const datasets12 = [{
            label: 'Valor',
            data: data.map(item => item.value1), // Asociar cada valor con su correspondiente label
            backgroundColor: colorsChart12 // Usar el color correspondiente según la posición
        }];


        const data12 = {
            labels: labels12,
            datasets: datasets12
        };

        const config12 = {
            type: 'bar',
            data: data12,
            options: {
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },
                indexAxis: 'y', // Esto hace que las barras sean horizontales               
                responsive: true,
                maintainAspectRatio: true,
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
                                    content: 'Patología Existente',
                                    enabled: true,
                                    rotation: 90, //
                                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                                    color: 'white',
                                    font: {
                                        size: 12,
                                    },
                                    xAdjust: -20,
                                }
                            }
                        }
                    }
                }
            }
        };
        new Chart(ctx12, config12);   
    }
});




