var progressBar10 = document.getElementById('progressBar10');
var canvas10 = document.getElementById('myChart10');
var ctx10 = canvas10.getContext('2d');
var datasets10 = [];
const colorsChart10 = generatePalette(10, 0.8);
var characterization10 = document.getElementById('characterization10');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart10?param=" + param,
    success: function (response) {    

        const labels10 = response.dimensions.map(item => item.label);
        const dataValues10 = response.dimensions.map(item => item.averageValue);

        const data10 = {
            labels: labels10,
            datasets: [{
                data: dataValues10,
                backgroundColor: colorsChart10,
            }]
        };

        const config10 = {
            type: 'polarArea',
            data: data10,
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

        new Chart(ctx10, config10);

        updateProgress(progressBar10, response.data.value1);
        characterization10.innerText = response.data.characterization;
        
    }
});
