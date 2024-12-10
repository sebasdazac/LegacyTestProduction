var canvas1 = document.getElementById('myChart1');
var ctx1 = canvas1.getContext('2d');

const labelsY = ['CFB', 'CPB', 'CEB', 'CFA', 'CPA', 'CEA'];
const labelsX = ['DEB', 'DPB', 'DFB', 'DEA', 'DPA', 'DFA'];

const visualLabelsY = ['Familia', 'Patrimonio', 'Empresa', 'Familia', 'Patrimonio', 'Empresa'];
const visualLabelsX = ['Empresa', 'Patrimonio', 'Familia', 'Empresa', 'Patrimonio', 'Familia'];

function ConfigMatrix(data, labelsX, labelsY) {
    let config = {
        type: 'matrix',
        data: data,
        options: {   
            animation: {
                onComplete: function () {
                    window.JSREPORT_READY_TO_START = true;
                }
            },
            pointLabel: {
                display: false
            },
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: false,
                datalabels: {
                    anchor: 'center',
                    align: 'center',
                    formatter: (value, context) => {
                        return context.dataset.label || '';
                    },
                    color: '#ffff',
                    labels: {
                        value: {
                            color: 'black'
                        }
                    }
                },
                tooltip: {
                    enabled: false
                }
            },
            scales: {
                x: {
                    display: true,
                    type: 'category',
                    labels: labelsX,
                    ticks: {
                        display: true,
                        callback: function (value, index) {
                            return visualLabelsX[index];
                        },

                        maxRotation: 90,
                        minRotation: 90
                    },
                    grid: {
                        display: false
                    }
                },
                x2: {
                    type: 'category',
                    display: true,
                    position: 'bottom',
                    labels: ['Bajo', 'Alto'],
                    title: {
                        display: true,
                        text: 'Direccionamiento Estrat\u00E9gico',
                        font: {
                            size: 16,
                            weight: 'bold'
                        }
                    },
                    ticks: {
                        display: true
                    },
                    grid: {
                        display: false
                    }
                },
                y: {
                    type: 'category',
                    labels: labelsY,
                    offset: true,
                    ticks: {
                        display: true,
                        callback: function (value, index) {
                            return visualLabelsY[index];
                        }
                    },
                    grid: {
                        display: false
                    }
                },
                y2: {
                    title: {
                        display: true,
                        text: 'Confianza',
                        font: {
                            size: 16,
                            weight: 'bold'
                        }
                    },
                    type: 'category',
                    labels: ['Baja', 'Alta'],
                    offset: true,
                    ticks: {
                        display: true
                    },
                    grid: {
                        display: false
                    }
                }
            },

        },

    };
    return config;
}

const data1 = {
    datasets: [
        {
            label: '',
            data: [
                { x: 'DEB', y: 'CEA', v: '35', ms: '' },
                { x: 'DPB', y: 'CEA', v: '35', ms: '' },
                { x: 'DFB', y: 'CEA', v: '35', ms: '' },
                { x: 'DEB', y: 'CPA', v: '35', ms: '' },
                { x: 'DPB', y: 'CPA', v: '35', ms: '' },
                { x: 'DFB', y: 'CPA', v: '35', ms: '' },
                { x: 'DEB', y: 'CFA', v: '35', ms: '' },
                { x: 'DPB', y: 'CFA', v: '35', ms: '' },
                { x: 'DFB', y: 'CFA', v: '35', ms: '' },
                { x: 'DEA', y: 'CEA', v: '35', ms: '' },
                { x: 'DPA', y: 'CEA', v: '35', ms: '' },
                { x: 'DFA', y: 'CEA', v: '35', ms: '' },
                { x: 'DEA', y: 'CPA', v: '35', ms: '' },
                { x: 'DPA', y: 'CPA', v: '35', ms: '' },
                { x: 'DFA', y: 'CPA', v: '35', ms: '' },
                { x: 'DEA', y: 'CFA', v: '35', ms: '' },
                { x: 'DPA', y: 'CFA', v: '35', ms: '' },
                { x: 'DFA', y: 'CFA', v: '35', ms: '' },

                { x: 'DEB', y: 'CEB', v: '35', ms: '' },
                { x: 'DPB', y: 'CEB', v: '35', ms: '' },
                { x: 'DFB', y: 'CEB', v: '35', ms: '' },
                { x: 'DEB', y: 'CPB', v: '35', ms: '' },
                { x: 'DPB', y: 'CPB', v: '35', ms: '' },
                { x: 'DFB', y: 'CPB', v: '35', ms: '' },
                { x: 'DEB', y: 'CFB', v: '35', ms: '' },
                { x: 'DPB', y: 'CFB', v: '35', ms: '' },
                { x: 'DFB', y: 'CFB', v: '35', ms: '' },

                { x: 'DEA', y: 'CEB', v: '35', ms: '' },
                { x: 'DPA', y: 'CEB', v: '35', ms: '' },
                { x: 'DFA', y: 'CEB', v: '35', ms: '' },
                { x: 'DEA', y: 'CPB', v: '35', ms: '' },
                { x: 'DPA', y: 'CPB', v: '35', ms: '' },
                { x: 'DFA', y: 'CPB', v: '35', ms: '' },
                { x: 'DEA', y: 'CFB', v: '35', ms: '' },
                { x: 'DPA', y: 'CFB', v: '35', ms: '' },
                { x: 'DFA', y: 'CFB', v: '35', ms: '' }
            ],
            backgroundColor(context) {
                const value = context.dataset.data[context.dataIndex].v;
                const alpha = value / 50;

                const colors = [
                    `rgba(255, 206, 86, ${alpha})`,  // amarillo
                    `rgba(75, 192, 192, ${alpha})`,  // verde
                    `rgba(255, 99, 132, ${alpha})`,  // rojo
                    `rgba(54, 162, 235, ${alpha})`   // azul
                ];

                const colorIndex = Math.floor(context.dataIndex / 9) % colors.length;

                return colors[colorIndex];
            },
            borderColor(context) {
                const value = context.dataset.data[context.dataIndex].v;
                const alpha = (value - 5) / 40;

            },
            borderWidth: 1,
            width: ({ chart }) => (chart.chartArea || {}).width / 6 - 1,
            height: ({ chart }) => (chart.chartArea || {}).height / 6 - 1,
            order: 1
        },
    ]
};


$.ajax({
    type: "GET",
    url: "/CharacterizationByCompany/GetChart1?param="+param,
    success: function (data) {
        data.forEach(item => {
            data1.datasets.push({
                label: item.label.charAt(0),
                data: [{ x: item.x, y: item.y, r: 10 }],
                type: 'bubble',
                borderColor: 'rgb(0,0,255)',
                backgroundColor: 'rgb(255,255,255)'
            });
        });
        const conf1 = ConfigMatrix(data1, labelsX, labelsY);
        var myChart = new Chart(ctx1, conf1);

        updateProgress(companyDirection, data[0].value1);
        updateProgress(companyTrust, data[0].value2);
        updateProgress(propertyDirection, data[1].value1);
        updateProgress(propertyTrust, data[1].value2);
        updateProgress(familyDirection, data[2].value1);
        updateProgress(familyTrust, data[2].value2);   

    }
    
});
