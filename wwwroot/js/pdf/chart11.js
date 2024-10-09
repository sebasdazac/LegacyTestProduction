var progressBar11 = document.getElementById('progressBar11');
var canvas11 = document.getElementById('myChart11');
canvas11.width = 500;
var ctx11 = canvas11.getContext('2d');
var datasets11 = [];
const colorsChart11 = generatePalette(10, 0.8);
var characterization11 = document.getElementById('characterization11');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart11?param=" + param,
    success: function (response) {

        const labels11 = response.dimensions.map(item => item.label);
        const dataValues11 = response.dimensions.map(item => item.averageValue);

        const data11 = {
            labels: labels11,
            datasets: [{
                data: dataValues11,
                backgroundColor: colorsChart11,
            }]
        };

        const config11 = {
            type: 'polarArea',
            data: data11,
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

        new Chart(ctx11, config11);

        updateProgress(progressBar11, response.data.value1);
        characterization11.innerText = response.data.characterization;
    }
});
