
Chart.register(ChartDataLabels);
function SaveOrUpdateForm(formId, controllerRoute, tableId) {
    var form = document.getElementById(formId);

    if (form) {
        var formData = new FormData(form);

        $.ajax({
            url: controllerRoute,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                $('#' + tableId).DataTable().ajax.reload();
                form.reset();
            },
            error: function (error) {
                console.error("Error en la solicitud AJAX: " + error);
            }
        });

    } else {
        console.error("El formulario con el ID proporcionado no existe.");
    }
}

function generatePalette(totalColors, alpha = 1.0) {
    const australiaPalette = [
        '#183D6B', // Deep Cove
        '#4834d4', // blurple
        '#22a6b3', // Greenland Green
        '#6ab04c', // Pure Apple
        '#eb4d4b', // Carmine Pink
        '#f0932b', // Quince Jelly
        '#f9ca24', // Turbo
        '#30336b', // Deep Koamaru
        '#7ed6df', // Middle Blue
        '#be2edd', // Steel Pink
        '#535c68', // Wizad Grey
        '#f6e58d', // Beekeeper
        '#ffbe76', // #Necatrine
        '#badc58', // Junebud
        '#95afc0', // Light Gray (neutral, not on the spectrum)
    ];


    const palette = [];

    for (let i = 0; i < totalColors; i++) {
        const colorIndex = i % australiaPalette.length;
        const color = hexToRgba(australiaPalette[colorIndex], alpha);
        palette.push(color);
    }

    return palette;
}

function hexToRgba(hex, alpha) {
    const bigint = parseInt(hex.slice(1), 16);
    const r = (bigint >> 16) & 255;
    const g = (bigint >> 8) & 255;
    const b = bigint & 255;

    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
}

function renderAccordion(data, containerId) {
    const accordionContainer = document.getElementById(containerId);
    accordionContainer.innerHTML = '';

    // Si data no es un array, conviértelo en un array de un solo elemento
    if (!Array.isArray(data)) {
        data = [data];
    }

    const accordion = document.createElement('div');
    accordion.className = 'accordion accordion-flush';

    data.forEach((item, index) => {
        const headerId = `flush-collapse-${containerId}-${index + 1}`;
        const headingId = `flush-heading-${containerId}-${index + 1}`;
        const collapsedClass = index !== 0 ? 'collapsed' : ''; // Colapsa las demás secciones
        const activeClass = index === 0 ? 'bg-blue3 text-white' : 'bg-blue1 text-white'; // Clases según el estado
        const showClass = index === 0 ? 'show' : ''; // Muestra la primera sección
        const chevronClass = index === 0 ? 'bi-chevron-down' : 'bi-chevron-up'; // Chevron hacia abajo solo en el primer elemento

        const accordionItem = document.createElement('div');
        accordionItem.className = 'accordion-item mb-2 border';


        const textColorClass = item.characterization.includes("[Presente]") ? "text-warning" : "fw-bold";

        const header = `
            <h2 class="accordion-header" id="${headingId}">
                <button class="accordion-button ${collapsedClass} ${activeClass}" type="button" data-bs-toggle="collapse" data-bs-target="#${headerId}" aria-expanded="${index === 0}" aria-controls="${headerId}">
                    <span class="${textColorClass}">${item.characterization}</span>
                    <i class="bi ${chevronClass} ms-auto"></i> <!-- Ícono de Bootstrap Icons -->
                </button>
            </h2>
        `;

        let effectsContent = '<div class="col-md-6"><h6 class="fw-bolder text-blue">Efectos</h6>';
        item.effects.forEach(effect => {
            effectsContent += `<p class="text-black text-justify"><strong>${effect.effectType}:</strong> ${effect.effect}</p>`;
        });
        effectsContent += '</div>';

        let recommendationsContent = '<div class="col-md-6"><h6 class="fw-bolder text-blue">Recomendaciones</h6>';
        item.recommendations.forEach(recommendation => {
            recommendationsContent += `<p class="text-black text-justify"><strong>${recommendation.recomendationType}:</strong> ${recommendation.recomendation}</p>`;
        });
        recommendationsContent += '</div>';

        const body = `
            <div id="${headerId}" class="accordion-collapse collapse ${showClass}" aria-labelledby="${headingId}">               
                <div class="accordion-body">
                    <div class="row">
                        ${effectsContent}
                        ${recommendationsContent}
                    </div>
                </div>
            </div>
        `;

        accordionItem.innerHTML = header + body;
        accordion.appendChild(accordionItem);
    });

    accordionContainer.appendChild(accordion);

    // Añadir evento para cambiar clases y comportamientos
    const accordionButtons = accordionContainer.querySelectorAll('.accordion-button');
    accordionButtons.forEach(button => {
        const icon = button.querySelector('i');

        button.addEventListener('click', function () {
            accordionButtons.forEach(btn => {
                const btnIcon = btn.querySelector('i');
                btn.classList.remove('bg-blue3', 'bg-blue1', 'text-white');
                btn.classList.add('bg-blue1', 'text-white'); // Inactivo por defecto
                btnIcon.classList.remove('bi-chevron-down');
                btnIcon.classList.add('bi-chevron-up'); // Ícono hacia arriba por defecto
            });

            if (!this.classList.contains('collapsed')) {
                this.classList.remove('bg-blue1');
                this.classList.add('bg-blue3', 'text-white'); // Activo
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
}

function renderDimensionAccordion(data, containerId, progressProperty, inverse) {

    const accordionContainer = document.getElementById(containerId);
    accordionContainer.innerHTML = '';

    // Si data no es un array, conviértelo en un array de un solo elemento
    if (!Array.isArray(data)) {
        data = [data];
    }

    const accordion = document.createElement('div');
    accordion.className = 'accordion accordion-flush';

    data.forEach((item, index) => {
        const headerId = `flush-collapse-${containerId}-${index + 1}`;
        const headingId = `flush-heading-${containerId}-${index + 1}`;
        const collapsedClass = index !== 0 ? 'collapsed' : ''; // Colapsa las demás secciones
        const activeClass = index === 0 ? 'bg-blue3 text-white' : 'bg-blue1 text-white'; // Clases según el estado
        const showClass = index === 0 ? 'show' : ''; // Muestra la primera sección
        const chevronClass = index === 0 ? 'bi-chevron-down' : 'bi-chevron-up'; // Chevron hacia abajo solo en el primer elemento

        const accordionItem = document.createElement('div');
        accordionItem.className = 'accordion-item mb-2 border';

        const textColorClass = item.characterization.includes("[Presente]") ? "text-warning" : "fw-bold";

        const progressValue = item[progressProperty] || 0;

        const header = `
            <h2 class="accordion-header bg-blue1" id="${headingId}">
                <div class="row">
                    <div class="col-md-2 d-flex align-items-center justify-content-center">
                        <div class="progress ms-4" role="progressbar" aria-label="Example with label"
                             aria-valuenow="${progressValue}" aria-valuemin="0" aria-valuemax="100" style="height: 1.5rem;">
                            <div class="progress-bar" style="width: ${progressValue}%">${progressValue}%</div>
                        </div>
                    </div>
                    <div class="col-md-10">
                        <button class="accordion-button ${collapsedClass} ${activeClass}" type="button" data-bs-toggle="collapse"
                                data-bs-target="#${headerId}" aria-expanded="${index === 0}" aria-controls="${headerId}">
                            <span class="${textColorClass}">${item.characterization}</span>
                            <i class="bi ${chevronClass} ms-auto"></i>          
                        </button>
                    </div>
                </div>
            </h2>        
        `;

        let effectsContent = '<div class="col-md-6"><h6 class="fw-bolder text-blue">Efectos</h6>';
        item.effects.forEach(effect => {
            effectsContent += `<p class="text-black text-justify"><strong>${effect.effectType}:</strong> ${effect.effect}</p>`;
        });
        effectsContent += '</div>';

        let recommendationsContent = '<div class="col-md-6"><h6 class="fw-bolder text-blue">Recomendaciones</h6>';
        item.recommendations.forEach(recommendation => {
            recommendationsContent += `<p class="text-black text-justify"><strong>${recommendation.recomendationType}:</strong> ${recommendation.recomendation}</p>`;
        });
        recommendationsContent += '</div>';

       

        const body = `
            <div id="${headerId}" class="accordion-collapse collapse ${showClass}" aria-labelledby="${headingId}">       
                <div class="accordion-body">
                    <div class="row">
                        ${effectsContent}
                        ${recommendationsContent}
                    </div>
                </div>
            </div>
        `;

        accordionItem.innerHTML = header + body;
        accordion.appendChild(accordionItem);
    });

    accordionContainer.appendChild(accordion);

    // Añadir evento para cambiar clases y comportamientos
    const accordionButtons = accordionContainer.querySelectorAll('.accordion-button');
    accordionButtons.forEach(button => {
        const icon = button.querySelector('i');

        button.addEventListener('click', function () {
            accordionButtons.forEach(btn => {
                const btnIcon = btn.querySelector('i');
                btn.classList.remove('bg-blue3', 'bg-blue1', 'text-white');
                btn.classList.add('bg-blue1', 'text-white'); // Inactivo por defecto
                btnIcon.classList.remove('bi-chevron-down');
                btnIcon.classList.add('bi-chevron-up'); // Ícono hacia arriba por defecto
            });

            if (!this.classList.contains('collapsed')) {
                this.classList.remove('bg-blue1');
                this.classList.add('bg-blue3', 'text-white'); // Activo
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


function renderOnlyRecommendationAccordion(data, containerId, progressProperty, inverse) {
    const accordionContainer = document.getElementById(containerId);
    accordionContainer.innerHTML = '';

    // Si data no es un array, conviértelo en un array de un solo elemento
    if (!Array.isArray(data)) {
        data = [data];
    }

    const accordion = document.createElement('div');
    accordion.className = 'accordion accordion-flush';

    data.forEach((item, index) => {
        const headerId = `flush-collapse-${containerId}-${index + 1}`;
        const headingId = `flush-heading-${containerId}-${index + 1}`;
        const collapsedClass = index !== 0 ? 'collapsed' : ''; // Colapsa las demás secciones
        const activeClass = index === 0 ? 'bg-blue3 text-white' : 'bg-blue1 text-white'; // Clases según el estado
        const showClass = index === 0 ? 'show' : ''; // Muestra la primera sección
        const chevronClass = index === 0 ? 'bi-chevron-down' : 'bi-chevron-up'; // Chevron hacia abajo solo en el primer elemento

        const accordionItem = document.createElement('div');
        accordionItem.className = 'accordion-item border mb-2';

        const progressValue = item[progressProperty] || 0;

        const header = `
            <h2 class="accordion-header bg-blue1" id="${headingId}">
                <div class="row">
                    <div class="col-md-2 d-flex align-items-center justify-content-center">
                        <div class="progress ms-4" role="progressbar" aria-label="Example with label"
                             aria-valuenow="${progressValue}" aria-valuemin="0" aria-valuemax="100" style="height: 1.5rem;">
                            <div class="progress-bar" style="width: ${progressValue}%">${progressValue}%</div>
                        </div>
                    </div>
                    <div class="col-md-10">
                        <button class="accordion-button ${collapsedClass} ${activeClass}" type="button" data-bs-toggle="collapse"
                                data-bs-target="#${headerId}" aria-expanded="${index === 0}" aria-controls="${headerId}">
                            <span>${item.characterization}</span>
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
        accordion.appendChild(accordionItem);
    });

    accordionContainer.appendChild(accordion);

    // Añadir evento para cambiar clases y comportamientos
    const accordionButtons = accordionContainer.querySelectorAll('.accordion-button');
    accordionButtons.forEach(button => {
        const icon = button.querySelector('i');

        button.addEventListener('click', function () {
            accordionButtons.forEach(btn => {
                const btnIcon = btn.querySelector('i');
                btn.classList.remove('bg-blue3', 'bg-blue1', 'text-white');
                btn.classList.add('bg-blue1', 'text-white'); // Inactivo por defecto
                btnIcon.classList.remove('bi-chevron-down');
                btnIcon.classList.add('bi-chevron-up'); // Ícono hacia arriba por defecto
            });

            if (!this.classList.contains('collapsed')) {
                this.classList.remove('bg-blue1');
                this.classList.add('bg-blue3', 'text-white'); // Activo
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

function updateProgress(progressBar, newValue) {
    progressBar.setAttribute('aria-valuenow', newValue);
    var progressBarInner = progressBar.querySelector('.progress-bar');
    progressBarInner.style.width = newValue + '%';
    progressBarInner.textContent = newValue + '%';


    // Remover clases previas de color
    progressBarInner.classList.remove('bg-yellow', 'bg-blue', 'bg-green');

    // Asignar nueva clase de color según el valor
    if (newValue <= 40) {
        progressBarInner.classList.add('bg-yellow');
    } else if (newValue < 70) {
        progressBarInner.classList.add('bg-blue');
    } else {
        progressBarInner.classList.add('bg-green');
    }
}

function updateProgressInverse(progressBar, newValue) {
    progressBar.setAttribute('aria-valuenow', newValue);
    var progressBarInner = progressBar.querySelector('.progress-bar');
    progressBarInner.style.width = newValue + '%';
    progressBarInner.textContent = newValue + '%';


    // Remover clases previas de color
    progressBarInner.classList.remove('bg-green', 'bg-blue', 'bg-yellow');

    // Asignar nueva clase de color según el valor
    if (newValue <= 40) {
        progressBarInner.classList.add('bg-yellow');
    } else if (newValue < 70) {
        progressBarInner.classList.add('bg-blue');
    } else {
        progressBarInner.classList.add('bg-green');
    }
}



function getRandomColor() {

    const blueValue = Math.floor(Math.random() * 156);
    const greenValue = Math.floor(Math.random() * 256);
    const alphaValue = 1;

    return `rgba(0, ${greenValue}, ${blueValue}, ${alphaValue})`;
}



