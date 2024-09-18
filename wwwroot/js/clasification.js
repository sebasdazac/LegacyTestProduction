var datatable;
function GetAllClasification() {
    _Clasifications = [];

    datatable = $('#tblClasification').DataTable({
        dom: 'fBrtlp',
        ajax: {
            type: 'GET',
            url: '/Form/GetAllClasification',
            dataSrc: function (data) {
                return data;
            }
        },
        columns: [
            { data: 'id' },
            { data: 'idCriterion' },
            { data: 'criterionText' },
            { data: 'clasification' },
            { data: 'min' },
            { data: 'max' },
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

function GetListCriterion() {
    // Obtener los datos de los formularios desde el controlador
    $.get('/Form/GetListCriterion', function (data) {
        console.log(data);
        // Limpiar el select
        $('#IdCriterion').empty();

        // Llenar el select con los datos obtenidos
        $.each(data, function (index, item) {
            $('#IdCriterion').append($('<option>', {
                value: item.id,
                text: item.criterionText
            }));
        });
    });
}


$(document).ready(function () {
    GetListCriterion();
    GetAllClasification();
});