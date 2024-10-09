var canvas4 = document.getElementById('myChart4');
var ctx4 = canvas4.getContext('2d');
var datasets4 = [];
const colorsChart4 = generatePalette(10, 0.8);

$.ajax({
    type: "GET",
    url: "/CharacterizationByCompany/GetChart4?param=" + param, 
    success: function (data) {     


        const labels4 = data.map(item => item.label);

        const datasets4 = [{
            label: 'Valor',
            data: data.map(item => item.value1), // Asociar cada valor con su correspondiente label
            backgroundColor: colorsChart4 // Usar el color correspondiente según la posición
        }];


        const data4 = {
            labels: labels4,
            datasets: datasets4
        };

        const config4 = {
            type: 'bar',
            data: data4,
            options: {
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },
                indexAxis: 'y', 
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
                        ticks: {
                            font: {
                                size: 10
                            }
                        }
                     
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
                                    content: 'Trampa Existente',
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
        new Chart(ctx4, config4);    


        data.forEach(item => {
            const chartElement = document.getElementById(`progress-${item.idCharacterization}`);
            if (chartElement) {
                chartElement.hidden = false;

                updateProgress(chartElement, item.value1);
            }
        });

    }
});




