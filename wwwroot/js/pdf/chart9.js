var progressBar9 = document.getElementById('progressBar9');
var canvas9 = document.getElementById('myChart9');
var ctx9 = canvas9.getContext('2d');
var datasets9 = [];
const colorsChart9 = generatePalette(10, 0.8);
var characterization9 = document.getElementById('characterization9');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart9?param=" + param,
    success: function (response) {


        const labels9 = response.dimensions.map(item => item.label);
        const dataValues9 = response.dimensions.map(item => item.averageValue);

        const data9 = {
            labels: labels9,
            datasets: [{
                data: dataValues9,
                backgroundColor: colorsChart9,
            }]
        };

        const config9 = {
            type: 'polarArea',
            data: data9,
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
                            text: '% Promedio',
                            color: 'white'
                        },
                        ticks: {
                            color: 'white',    
                            callback: function (value) {
                                return value + '%'; // Agrega el símbolo de porcentaje a los ticks
                            }
                        }
                    }
                },
                plugins: {
                    legend: {
                        position: 'right',
                    },
                    datalabels: {
                        color: 'black', 
                        formatter: function (value) {
                            return value + '%'; 
                        }
                    },

                }
            }
        };

        new Chart(ctx9, config9);

        updateProgress(progressBar9, response.data.value1);
        characterization9.innerText = response.data.characterization;
    }
});
