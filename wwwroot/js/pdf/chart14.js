var progressBar14 = document.getElementById('progressBar14');
var canvas14 = document.getElementById('myChart14');
canvas14.width = 400;
var ctx14 = canvas14.getContext('2d');
var datasets14 = [];
const colorsChart14 = generatePalette(10, 0.8);


$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart14?param=" + param,
    success: function (response) {

        const labels14 = response.dimensions.map(item => item.label);
        const dataValues14 = response.dimensions.map(item => item.averageValue);

        const data14 = {
            labels: labels14,
            datasets: [{
                data: dataValues14,
                backgroundColor: colorsChart14,
            }]
        };

        const config14 = {
            type: 'polarArea',
            data: data14,
            options: {
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },       
                responsive: true,
                maintainAspectRatio: true,
                scales: {
                    r: {
                     
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

        new Chart(ctx14, config14);

        updateProgress(progressBar14, response.data.value1);
        characterization14.innerText = response.data.characterization;       
    }
});
