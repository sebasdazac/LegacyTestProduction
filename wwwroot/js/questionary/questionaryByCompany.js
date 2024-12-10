document.addEventListener("DOMContentLoaded", function () {
    initializeTabs();
});
function initializeTabs() {
    var stepLinks = document.querySelectorAll(".step-item-link");
    var btnNext = document.getElementById("btnNext");
    var btnPrev = document.getElementById("btnPrev");
    var currentTabIndex = 0;

    stepLinks.forEach(function (link, index) {
        link.addEventListener("click", function (event) {
            event.preventDefault(); 

            var stepItems = document.querySelectorAll(".step-item");
            stepItems.forEach(function (item) {
                item.classList.remove("active");
            });

            this.parentNode.classList.add("active");

            var tabContents = document.querySelectorAll(".tab-pane");
            tabContents.forEach(function (content) {
                if (content.id === link.getAttribute("href").substring(1)) {
                    content.classList.add("show", "active");
                } else {
                    content.classList.remove("show", "active");
                }
            });

            currentTabIndex = index;
            updateButtonVisibility();
        });
    });

    
    function updateButtonVisibility() {
        if (currentTabIndex === 0) {
            btnPrev.style.display = "none";
        } else {
            btnPrev.style.display = "inline-block"; 
        }

        if (currentTabIndex === stepLinks.length - 1) {
            btnNext.style.display = "none";
        } else {
            btnNext.style.display = "inline-block"; 
        }
    }

    btnNext.addEventListener("click", function (event) {
        event.preventDefault();
        currentTabIndex++;
        if (currentTabIndex >= stepLinks.length) {
            currentTabIndex = 0;
        }
        activateTab(currentTabIndex);
    });


    btnPrev.addEventListener("click", function (event) {
        event.preventDefault();
        currentTabIndex--; 
        if (currentTabIndex < 0) {
            currentTabIndex = stepLinks.length - 1; 
        }
        activateTab(currentTabIndex);
    });

    function activateTab(index) {        
        var stepItems = document.querySelectorAll(".step-item");
        stepItems.forEach(function (item) {
            item.classList.remove("active");
        });

        stepLinks[index].parentNode.classList.add("active");
        var tabContents = document.querySelectorAll(".tab-pane");
        tabContents.forEach(function (content, i) {
            if (i === index) {
                content.classList.add("show", "active");
            } else {
                content.classList.remove("show", "active");
            }
        });
        updateButtonVisibility();
    }
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
            Swal.fire({
                title: "Oh Oh!",
                text: "Hubo un error al procesar el formulario: " + xhr.responseText,
                icon: "error"
            });
        }
    });
}

