var progressBar17 = document.getElementById('progressBar17');
var canvas17 = document.getElementById('myChart17');
var ctx17 = canvas17.getContext('2d');
var datasets17 = [];
const colorsChart17 = generatePalette(10, 0.8);

var characterization17 = document.getElementById('characterization17');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart17",
    success: function (response) {

        const labels17 = response.dimensions.map(item => item.label);
        const dataValues17 = response.dimensions.map(item => item.averageValue);

        const data17 = {
            labels: labels17,
            datasets: [{
                data: dataValues17,
                backgroundColor: colorsChart17,
            }]
        };

        const config17 = {
            type: 'polarArea',
            data: data17,
            options: {
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },      
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    r: {
                        // suggestedMax: 170,
                        title: {
                            display: true,
                            text: 'Diagnóstico por dimensiones para la estructura familiar',

                        },
                        ticks: {
                            color: 'white' // Cambia el color de los ticks a blanco
                        }
                    }
                },
                plugins: {
                    legend: {
                        position: 'right',
                    },
                    datalabels: {
                        color: 'black', // Cambia el color de los datalabels

                    },

                }
                
            }
          
        };

        new Chart(ctx17, config17);

        updateProgress(progressBar17, response.data.value1);
        characterization17.innerText = response.data.characterization;
        renderAccordion(response.data, 'effectsAndRecommendationsAlert17');
        
    }
});
