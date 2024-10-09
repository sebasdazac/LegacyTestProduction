var progressBar7 = document.getElementById('progressBar7');
var canvas7 = document.getElementById('myChart7');
var ctx7 = canvas7.getContext('2d');
var datasets7 = [];
const colorsChart7 = generatePalette(14, 0.8);
var characterization7 = document.getElementById('characterization7');


$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart7?param=" + param,
    success: function (response) {
      
        const labels7 = response.dimensions.map(item => item.label);

        const datasets7 = [{
            label: 'Valor',
            data: response.dimensions.map(item => item.averageValue), // Asociar cada valor con su correspondiente label
            backgroundColor: colorsChart7 // Usar el color correspondiente según la posición
        }];

        const data7 = {
            labels: labels7,
            datasets: datasets7
        };


        const config7 = {
            type: 'bar',
            data: data7,
            options: {
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },
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
                 
                }
            }
        };

        new Chart(ctx7, config7);

        updateProgress(progressBar7, response.data.value1);
        characterization7.innerText = response.data.characterization; 
   

    }
});