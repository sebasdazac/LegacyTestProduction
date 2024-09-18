var idPerson = document.getElementById('IdPerson').value;
var idCompany = document.getElementById('idCompany').value;
var idPlan = document.getElementById('idPlan').value;


function LoadDynamicForm(formId) {    
    $.ajax({
        type: "POST",
        url: `/Questionary/DynamicForm?id=${formId}`,
        success: function (response) {
            window.location.href = '/MyForm';
            $('#partialZone').html(response);            
        },
        error: function (error) {
            console.log("Error al cargar el formulario dinámico:", error);
        }
    });
}

