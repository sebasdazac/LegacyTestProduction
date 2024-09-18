var datatable;
function GetAllQuestion() {
    _Questions = [];

    datatable = $('#tblQuestion').DataTable({
        dom: 'fBrtlp',
        ajax: {
            type: 'GET',
            url: '/Form/GetAllQuestion',
            dataSrc: function (data) {
                return data;
            }
        },
        columns: [
            { data: 'id' },
            { data: 'questionText' },
            { data: 'idForm' },
            { data: 'nameForm' },
            { data: 'questionParent' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class='text-center'> 
                <button class="btn btn-outline-secondary p-1" style="cursor: pointer" onclick="Edit(${data})"><i class='bx bx-edit'></i></button>                              
                <button class="btn btn-primary p-1" style="cursor: pointer" onclick="Delete(${data})"><i class='bx bx-trash' ></i></button>                  
            </div>`;
                }
            }
        ]
    });

    datatable.column(2).visible(false);
}

function GetListForm() {
    // Obtener los datos de los formularios desde el controlador
    $.get('/Form/GetListForm', function (data) {
        console.log(data);
        // Limpiar el select
        $('#IdForm').empty();

        // Llenar el select con los datos obtenidos
        $.each(data, function (index, item) {
            $('#IdForm').append($('<option>', {
                value: item.id,
                text: item.nameForm
            }));
        });
    });
}

function GetListQuestionParent() {
    $.get('/Form/GetListQuestionParent', function (data) {
        // Limpiar el select
        $('#QuestionParent').empty();

        // Llenar el select con los IDs obtenidos
        $.each(data, function (index, id) {
            $('#QuestionParent').append($('<option>', {
                value: id,
                text: id
            }));
        });
    });
}

$(document).ready(function () {
    GetListForm();
    GetListQuestionParent();
    GetAllQuestion();
});