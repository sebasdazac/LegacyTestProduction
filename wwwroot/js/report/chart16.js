var progressBar16 = document.getElementById('progressBar16');
var canvas16 = document.getElementById('myChart16');
var ctx16 = canvas16.getContext('2d');
var datasets16 = [];
const colorsChart16 = generatePalette(10, 0.8);

var characterization16 = document.getElementById('characterization16');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart16",
    success: function (response) {

        const labels16 = response.dimensions.map(item => item.label);
        const dataValues16 = response.dimensions.map(item => item.averageValue);

        const data16 = {
            labels: labels16,
            datasets: [{
                data: dataValues16,
                backgroundColor: colorsChart16,
            }]
        };

        const config16 = {
            type: 'polarArea',
            data: data16,
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
                        // suggestedMax: 160,
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

        new Chart(ctx16, config16);

        updateProgress(progressBar16, response.data.value1);
        characterization16.innerText = response.data.characterization;
        renderAccordion(response.data, 'effectsAndRecommendationsAlert16');

      
    }
});
