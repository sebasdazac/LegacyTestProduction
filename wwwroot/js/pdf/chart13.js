var progressBar13 = document.getElementById('progressBar13');
var canvas13 = document.getElementById('myChart13');
var ctx13 = canvas13.getContext('2d');
var datasets13 = [];
const colorsChart13 = generatePalette(10, 0.8);
var characterization13 = document.getElementById('characterization13');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart13?param=" + param,
    success: function (response) {

        const labels13 = response.dimensions.map(item => item.label);
        const dataValues13 = response.dimensions.map(item => item.averageValue);

        const data13 = {
            labels: labels13,
            datasets: [{
                data: dataValues13,
                backgroundColor: colorsChart13,
            }]
        };

        const config13 = {
            type: 'polarArea',
            data: data13,
            options: {   
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },
                responsive:true ,
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
                                return value + '%';
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

        new Chart(ctx13, config13);

        updateProgress(progressBar13, response.data.value1);
        characterization13.innerText = response.data.characterization;
        
    }
});
