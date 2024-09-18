document.addEventListener("DOMContentLoaded", function () {
    var selects = document.querySelectorAll('.selectColor');
    selects.forEach(function (select) {
        select.addEventListener('change', function () {
            var selectedOption = this.options[this.selectedIndex];
            var selectedText = selectedOption.text;

            // Elimina clases de colores existentes en el select
            this.classList.remove('color-1', 'color-2', 'color-3', 'color-4', 'color-5');

            // Asigna la clase de color según la opción seleccionada
            switch (selectedText) {
                case 'Muy en desacuerdo':
                    this.classList.add('color-1');
                    break;
                case 'En desacuerdo':
                    this.classList.add('color-2');
                    break;
                case 'Neutral':
                    this.classList.add('color-3');
                    break;
                case 'De acuerdo':
                    this.classList.add('color-4');
                    break;
                case 'Muy de acuerdo':
                    this.classList.add('color-5');
                    break;
                // Añade más casos según sea necesario
            }
        });

        // Inicialmente, aplica la clase según la opción seleccionada
        var initialOption = select.options[select.selectedIndex];
        var initialText = initialOption.text;

        switch (initialText) {
            case 'Muy en desacuerdo':
                select.classList.add('color-1');
                break;
            case 'En desacuerdo':
                select.classList.add('color-2');
                break;
            case 'Neutral':
                select.classList.add('color-3');
                break;
            case 'De acuerdo':
                select.classList.add('color-4');
                break;
            case 'Muy de acuerdo':
                select.classList.add('color-5');
                break;
            // Añade más casos según sea necesario
        }
    });
    // Aquí puedes continuar con el resto de tu código JavaScript
    initializeTabs();
});



function initializeTabs() {
    // Capturar todos los enlaces de tabs
    var stepLinks = document.querySelectorAll(".step-item-link");

    // Capturar los botones de Siguiente y Atrás
    var btnNext = document.getElementById("btnNext");
    var btnPrev = document.getElementById("btnPrev");

    // Variable para almacenar el índice actual del tab
    var currentTabIndex = 0;

    // Agregar un evento click a cada enlace de tab
    stepLinks.forEach(function (link, index) {
        link.addEventListener("click", function (event) {
            event.preventDefault(); // Evitar el comportamiento predeterminado del enlace

            // Quitar la clase "active" de todos los tabs
            var stepItems = document.querySelectorAll(".step-item");
            stepItems.forEach(function (item) {
                item.classList.remove("active");
            });

            // Agregar la clase "active" al tab seleccionado
            this.parentNode.classList.add("active");

            // Mostrar el contenido del tab seleccionado y ocultar los demás
            var tabContents = document.querySelectorAll(".tab-pane");
            tabContents.forEach(function (content) {
                if (content.id === link.getAttribute("href").substring(1)) {
                    content.classList.add("show", "active");
                } else {
                    content.classList.remove("show", "active");
                }
            });

            // Actualizar el índice actual del tab
            currentTabIndex = index;
            // Actualizar la visibilidad de los botones de Siguiente y Atrás
            updateButtonVisibility();
        });
    });

    // Función para mostrar u ocultar los botones de Siguiente y Atrás según el tab actual
    function updateButtonVisibility() {
        if (currentTabIndex === 0) {
            btnPrev.style.display = "none"; // Ocultar botón Atrás en el primer tab
        } else {
            btnPrev.style.display = "inline-block"; // Mostrar botón Atrás si no es el primer tab
        }

        if (currentTabIndex === stepLinks.length - 1) {
            btnNext.style.display = "none"; // Ocultar botón Siguiente en el último tab
        } else {
            btnNext.style.display = "inline-block"; // Mostrar botón Siguiente si no es el último tab
        }
    }

    // Agregar un evento click al botón de Siguiente
    btnNext.addEventListener("click", function (event) {
        event.preventDefault();
        currentTabIndex++; // Incrementar el índice para avanzar al siguiente tab
        if (currentTabIndex >= stepLinks.length) {
            currentTabIndex = 0; // Volver al primer tab si llegamos al final
        }
        activateTab(currentTabIndex);
    });

    // Agregar un evento click al botón de Atrás
    btnPrev.addEventListener("click", function (event) {
        event.preventDefault();
        currentTabIndex--; // Decrementar el índice para retroceder al tab anterior
        if (currentTabIndex < 0) {
            currentTabIndex = stepLinks.length - 1; // Volver al último tab si estamos en el primer tab
        }
        activateTab(currentTabIndex);
    });

    // Función para activar el tab correspondiente al índice
    function activateTab(index) {
        // Quitar la clase "active" de todos los tabs
        var stepItems = document.querySelectorAll(".step-item");
        stepItems.forEach(function (item) {
            item.classList.remove("active");
        });

        // Agregar la clase "active" al tab seleccionado
        stepLinks[index].parentNode.classList.add("active");

        // Mostrar el contenido del tab seleccionado y ocultar los demás
        var tabContents = document.querySelectorAll(".tab-pane");
        tabContents.forEach(function (content, i) {
            if (i === index) {
                content.classList.add("show", "active");
            } else {
                content.classList.remove("show", "active");
            }
        });

        // Actualizar la visibilidad de los botones de Siguiente y Atrás
        updateButtonVisibility();
    }

    // Mostrar u ocultar los botones de Siguiente y Atrás al cargar la página
    updateButtonVisibility();
}

function SendForm(event) {
    event.preventDefault();
    let allSelectsSelected = true;
    $("[id^='question-']").each(function () {
        let answerValue = $(this).val();
        if (!answerValue) {
            allSelectsSelected = false;
            return false; 
        }
    });

    if (!allSelectsSelected) {
        // Mostrar alerta de error si no todos los select están seleccionados
        Swal.fire({
            title: "Error!",
            text: "Por favor, selecciona una opción en todos los campos",
            icon: "error"
        });
        return; 
    }


    Swal.fire({
        icon: "warning",
        title: "Enviando formulario al controlador...",
        showConfirmButton: false,
        timer: 1500
    });

    let formData = [];

    $("[id^='question-']").each(function () {
        let questionId = $(this).attr('id').split('-')[1];
        let answerValue = $(this).val();

        if (answerValue) {
            let $questionBlock = $(this).closest('.row');
            let criterionId = $questionBlock.find("input[name='IdCriterio']").val();

            formData.push({
                IdQuestion: questionId,
                IdAnswer: answerValue,
                IdCriterio: criterionId
            });
        }
    });

    $.ajax({
        url: '/Questionary/ProcessForm',
        type: 'POST',
        data: JSON.stringify(formData),
        contentType: 'application/json',
        success: function (response) {
            Swal.fire({
                title: "Muy Bien!",
                text: response,
                icon: "success"
            });

        },
        error: function (xhr, status, error) {
            // Mostrar el mensaje de error en el frontend
            Swal.fire({
                title: "Oh Oh!",
                text: "Hubo un error al procesar el formulario: " + xhr.responseText,
                icon: "error"
            });
        }
    });
}

