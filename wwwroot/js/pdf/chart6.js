var canvas6 = document.getElementById('myChart6');
var ctx6 = canvas6.getContext('2d');
var datasets6 = [];
const colorsChart6 = generatePalette(10,0.8);

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart6?param=" + param, ,
    success: function (response) {

        const labels6 = response.dimensions.map(item => item.label);

        const datasets6 = [{
            label: 'Valor',
            data: response.dimensions.map(item => item.averageValue), // Asociar cada valor con su correspondiente label
            backgroundColor: colorsChart6 // Usar el color correspondiente según la posición
        }];


        const data6 = {
            labels: labels6,
            datasets: datasets6
        };


        const config6 = {
            type: 'bar',
            data: data6,
            options: {
                animation: {
                    onComplete: function () {
                        window.JSREPORT_READY_TO_START = true
                    }
                },
                indexAxis: 'y', // Esto hace que las barras sean horizontales              
                responsive: true,
                maintainAspectRatio: true,              
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
                            text: '% Promedio'
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
                    annotation: {
                        annotations: {
                            line1: {
                                type: 'line',
                                xMin: 70,
                                xMax: 70,
                                borderColor: 'red',
                                borderWidth: 2,
                                label: {
                                    content: 'Equipaje Existente',
                                    enabled: true,
                                    rotation: 90, //
                                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                                    color: 'white',
                                    font: {
                                        size: 12                                       
                                    },
                                    xAdjust: 20,
                                }
                            }
                        }
                    }
                }
            }
        };

        new Chart(ctx6, config6);
        renderEmotionalBaggage(response.data, 'effectsAndRecommendationsAlert6','value1', true);
    }
});


function renderEmotionalBaggage(data, containerId, progressProperty, inverse) {

    const accordionContainer = document.getElementById(containerId);
    accordionContainer.innerHTML = '';

    // Si data no es un array, conviértelo en un array de un solo elemento
    if (!Array.isArray(data)) {
        data = [data];
    }

    const rows = {
        firstRow: document.createElement('div'),
        secondRow: document.createElement('div'),
    };

    rows.firstRow.className = 'row m-5';
    rows.secondRow.className = 'row m-5';

    const accordions = {
        crianza: document.createElement('div'),
        valores: document.createElement('div'),
        creencias: document.createElement('div'),
        costumbres: document.createElement('div'),
    };

    accordions.crianza.className = 'accordion accordion-flush col-md-6';
    accordions.valores.className = 'accordion accordion-flush col-md-6';
    accordions.creencias.className = 'accordion accordion-flush col-md-6';
    accordions.costumbres.className = 'accordion accordion-flush col-md-6';

    data.forEach((item, index) => {
        const headerId = `flush-collapse-${containerId}-${index + 1}`;
        const headingId = `flush-heading-${containerId}-${index + 1}`;
        const collapsedClass = index !== 0 ? 'collapsed' : ''; // Colapsa las demás secciones
        const activeClass = index === 0 ? 'bg-blue2 text-white' : 'bg-blue1 text-white'; // Clases según el estado
        const showClass = index === 0 ? 'show' : ''; // Muestra la primera sección
        const chevronClass = index === 0 ? 'bi-chevron-down' : 'bi-chevron-up'; // Chevron hacia abajo solo en el primer elemento

        const accordionItem = document.createElement('div');
        accordionItem.className = 'accordion-item mb-2 border';

        const textColorClass = item.characterization.includes("Crianza") ? "fw-bolder text-primary" :
            item.characterization.includes("Valores") ? "fw-bolder text-success" :
                item.characterization.includes("Creencias") ? "fw-bolder text-info" :
                    item.characterization.includes("Costumbres") ? "fw-bolder text-danger" : "fw-bolder";

        // Obtener el valor de la propiedad de progreso
        const progressValue = item[progressProperty] || 0;

        const header = `
      <h2 class="accordion-header bg-blue1" id="${headingId}">
            <div class="row">
                <div class="col-md-3 d-flex align-items-center justify-content-center">
                    <div class="progress ms-4" role="progressbar" aria-label="Example with label"
                         aria-valuenow="${progressValue}" aria-valuemin="0" aria-valuemax="100" style="height: 1.5rem;">
                        <div class="progress-bar" style="width: ${progressValue}%">${progressValue}%</div>
                    </div>
                </div>
                <div class="col-md-9">
                    <button class="accordion-button ${collapsedClass} ${activeClass}" type="button" data-bs-toggle="collapse"
                            data-bs-target="#${headerId}" aria-expanded="${index === 0}" aria-controls="${headerId}">
                        <span class="${textColorClass}">${item.characterization}</span>
                        <i class="bi ${chevronClass} ms-auto"></i>          
                    </button>
                </div>
            </div>
        </h2>

        `;

        let recommendationsContent = '<div class="col-md-12"><h6 class="fw-bolder text-blue">Recomendaciones</h6>';
        item.recommendations.forEach(recommendation => {
            recommendationsContent += `<p class="text-black text-justify"><strong>${recommendation.recomendationType}:</strong> ${recommendation.recomendation}</p>`;
        });
        recommendationsContent += '</div>';

       

        const body = `
            <div id="${headerId}" class="accordion-collapse collapse ${showClass}" aria-labelledby="${headingId}">
              
                <div class="accordion-body">
                    <div class="row">                      
                        ${recommendationsContent}
                    </div>
                </div>
            </div>
        `;

        accordionItem.innerHTML = header + body;

        if (item.characterization.includes("Crianza")) {
            accordions.crianza.appendChild(accordionItem);
        } else if (item.characterization.includes("Valores")) {
            accordions.valores.appendChild(accordionItem);
        } else if (item.characterization.includes("Creencias")) {
            accordions.creencias.appendChild(accordionItem);
        } else if (item.characterization.includes("Costumbres")) {
            accordions.costumbres.appendChild(accordionItem);
        }
    });

    rows.firstRow.appendChild(accordions.crianza);
    rows.firstRow.appendChild(accordions.valores);
    rows.secondRow.appendChild(accordions.creencias);
    rows.secondRow.appendChild(accordions.costumbres);

    accordionContainer.appendChild(rows.firstRow);
    accordionContainer.appendChild(rows.secondRow);

    // Añadir evento para cambiar clases y comportamientos
    const accordionButtons = accordionContainer.querySelectorAll('.accordion-button');
    accordionButtons.forEach(button => {
        const icon = button.querySelector('i');

        button.addEventListener('click', function () {
            accordionButtons.forEach(btn => {
                const btnIcon = btn.querySelector('i');
                btn.classList.remove('bg-blue2', 'bg-blue1', 'text-white');
                btn.classList.add('bg-blue1', 'text-white'); // Inactivo por defecto
                btnIcon.classList.remove('bi-chevron-down');
                btnIcon.classList.add('bi-chevron-up'); // Ícono hacia arriba por defecto
            });

            if (!this.classList.contains('collapsed')) {
                this.classList.remove('bg-blue1');
                this.classList.add('bg-blue2', 'text-white'); // Activo
                icon.classList.remove('bi-chevron-up');
                icon.classList.add('bi-chevron-down'); // Cambia el ícono hacia abajo cuando se expande

                // Cerrar los otros acordeones
                accordionButtons.forEach(btn => {
                    if (btn !== this) {
                        const targetCollapse = document.querySelector(btn.getAttribute('data-bs-target'));
                        targetCollapse.classList.remove('show');
                        btn.classList.add('collapsed');
                        const btnIcon = btn.querySelector('i');
                        btnIcon.classList.remove('bi-chevron-down');
                        btnIcon.classList.add('bi-chevron-up'); // Cambiar el ícono de los otros acordeones a arriba
                    }
                });
            }
        });
    });

    // Inicializar barras de progreso en el valor de la propiedad proporcionada
    const progressBars = accordionContainer.querySelectorAll('.progress');
    progressBars.forEach(progressBar => {
        const progressValue = parseInt(progressBar.getAttribute('aria-valuenow'), 10);
        if (inverse) {
            updateProgressInverse(progressBar, progressValue);
        } else {
            updateProgress(progressBar, progressValue);
        }
    });
}
