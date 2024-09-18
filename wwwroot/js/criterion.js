var datatable;
function GetAllCriterion() {
    _Criterions = [];

    datatable = $('#tblCriterion').DataTable({
        dom: 'fBrtlp',
        ajax: {
            type: 'GET',
            url: '/Form/GetAllCriterion',
            dataSrc: function (data) {
                return data;
            }
        },
        columns: [
            { data: 'id' },           
            { data: 'idForm' },
            { data: 'nameForm' },
            { data: 'criterionText' },
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


$(document).ready(function () {
    GetListForm(); 
    GetAllCriterion();
});