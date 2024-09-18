// Función para mapear valores a letras según el vértice
function getLabelForValue(value, vertex) {
    const mapping = {
        'Empresa': {
            20: 'A',
            60: 'B',
            100: 'C'
        },
        'Propiedad': {
            20: 'D',
            60: 'E',
            100: 'F'
        },
        'Familia': {
            20: 'G',
            60: 'H',
            100: 'I'
        }
    };

    return mapping[vertex][value] || '';
}

// Luego, dentro del éxito de tu AJAX y la configuración de Chart.js:
$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart18",
    success: function (data) {

        console.log(data);

        const labels = data.map(item => item.label);

        const dataset18 = {
            label: '', // Empty label to hide it from the legend
            data: data.map(item => item.value1),
            backgroundColor: 'rgba(100, 100, 100, 0.2)',
            borderWidth: 1,
            pointRadius: 0
        };

        const colors = {
            'Mono-negocio': {
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                pointBackgroundColor: 'rgba(255, 99, 132, 1)'
            },
            'Negocios relacionados': {
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                pointBackgroundColor: 'rgba(255, 99, 132, 1)'
            },
            'Negocios relacionados y no relacionados': {
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                pointBackgroundColor: 'rgba(255, 99, 132, 1)'
            },
            'Concentrada': {
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                pointBackgroundColor: 'rgba(54, 162, 235, 1)'
            },
            'Separada': {
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                pointBackgroundColor: 'rgba(54, 162, 235, 1)'
            },
            'Dispersa': {
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                pointBackgroundColor: 'rgba(54, 162, 235, 1)'
            },
            'Primera generación': {
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                pointBackgroundColor: 'rgba(75, 192, 192, 1)'
            },
            'Segunda generación': {
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                pointBackgroundColor: 'rgba(75, 192, 192, 1)'
            },
            'Tercera generación': {
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                pointBackgroundColor: 'rgba(75, 192, 192, 1)'
            }
        };

        const recommendationDatasets = data.flatMap(item => item.recommendations.map(rec => ({
            label: rec.recomendationType,
            data: labels.map(label => label === item.label ? item.value1 : 0),
            backgroundColor: colors[rec.recomendationType].backgroundColor,
            borderColor: colors[rec.recomendationType].borderColor,
            borderWidth: 1,
            pointBackgroundColor: colors[rec.recomendationType].pointBackgroundColor,
            pointBorderColor: '#fff',
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: colors[rec.recomendationType].borderColor,
            pointRadius: labels.map(label => label === item.label ? 15 : 0)
        })));

        const allDatasets = [dataset18, ...recommendationDatasets];

        const ctx = document.getElementById('myChart18').getContext('2d');
        const radarChart = new Chart(ctx, {
            type: 'radar',
            data: {
                labels: labels,
                datasets: allDatasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    r: {
                        suggestedMax: 100,
                        beginAtZero: true
                    }
                },
                elements: {
                    line: {
                        tension: 0.2
                    }
                },
                plugins: {
                    legend: {
                        labels: {
                            filter: function (legendItem, chartData) {
                                return legendItem.text !== '';
                            },
                            generateLabels: function (chart) {
                                const original = Chart.defaults.plugins.legend.labels.generateLabels;
                                const labelsOriginal = original.call(this, chart);
                                labelsOriginal.forEach(label => {
                                    if (label.text === '') {
                                        label.fillStyle = 'rgba(100, 100, 100, 0.2)';
                                    } else {
                                        const dataset = chart.data.datasets.find(dataset => dataset.label === label.text);
                                        label.fillStyle = dataset.pointBackgroundColor;
                                    }
                                });
                                return labelsOriginal;
                            }
                        }
                    },
                    datalabels: {
                        color: 'white',
                        formatter: function (value, context) {
                            const vertex = context.chart.data.labels[context.dataIndex];
                            return getLabelForValue(value, vertex);
                        }
                    }
                }
            }
        });

        renderOnlyRecommendationAccordion(data, 'effectsAndRecommendationsAlert18','value1', false);
    }
});



document.addEventListener("DOMContentLoaded", function () {
    // Obtener la tabla por su ID
    let table = document.getElementById("table-18");

    if (table) {
        // Insertar las nuevas columnas en el encabezado
        let headerRow = table.querySelector("thead tr");

        let newThCriterio = document.createElement("th");
        newThCriterio.classList.add("text-center", "bg-blue3", "text-white");
        newThCriterio.scope = "col";
        newThCriterio.innerText = "CRITERIO";
        headerRow.insertBefore(newThCriterio, headerRow.firstChild);

        let newThCategoria = document.createElement("th");
        newThCategoria.classList.add("text-center", "bg-blue3", "text-white");
        newThCategoria.scope = "col";
        newThCategoria.innerText = "CATEGORÍA";
        headerRow.insertBefore(newThCategoria, headerRow.children[1]);


        // Renombrar la columna 4
        headerRow.children[3].innerText = "ÓRGANOS DE GOBIERNO SUGERIDOS";

        // Iterar sobre las filas del cuerpo de la tabla y modificar el contenido
        let rows = table.querySelectorAll("tbody tr");
        let categoriaIndex = 0;

        rows.forEach(row => {
            let firstCellText = row.children[0].innerText;
            let criterio = firstCellText.split(":")[0]; // Obtener el criterio (ejemplo: Empresa)
            let categoria = String.fromCharCode(65 + categoriaIndex); // A, B, C, etc.

            // Insertar la columna de Criterio
            let newTdCriterio = document.createElement("td");
            newTdCriterio.classList.add("text-center", "p-3");
            newTdCriterio.innerText = criterio;
            row.insertBefore(newTdCriterio, row.firstChild);

            // Insertar la columna de Categoría
            let newTdCategoria = document.createElement("td");
            newTdCategoria.classList.add("text-center", "p-3");
            newTdCategoria.innerText = categoria;
            row.insertBefore(newTdCategoria, row.children[1]);

            // Actualizar la tercera columna (ex primera columna)
            row.children[2].innerText = firstCellText.replace(criterio + ": ", "");

            // Incrementar el índice de categoría
            categoriaIndex++;
        });

        let lastCriterio = '';
        let rowspan = 1;
        rows.forEach((row, i) => {
            let currentCriterio = row.children[0].innerText;
            if (currentCriterio === lastCriterio) {
                rowspan++;
                row.children[0].remove(); // Elimina la celda duplicada
                rows[i - rowspan + 1].children[0].rowSpan = rowspan; // Incrementa el rowspan de la celda combinada
            } else {
                lastCriterio = currentCriterio;
                rowspan = 1;
            }
        });
    } else {
        console.error("Tabla con ID 'table-18' no encontrada.");
    }
});


