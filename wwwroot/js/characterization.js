var datatable;
function GetAllCharacterization() {
    _Characterizations = [];

    datatable = $('#tblCharacterization').DataTable({
        dom: 'fBrtlp',
        ajax: {
            type: 'GET',
            url: '/Form/GetAllCharacterization',
            dataSrc: function (data) {
                return data;
            }
        },
        columns: [
            { data: 'id' },
            { data: 'idCriterion1' },
            { data: 'criterionText1' },
            { data: 'clasification1' },
            { data: 'min1' },
            { data: 'max1' },
            { data: 'idCriterion2' },
            { data: 'criterionText2' },
            { data: 'clasification2' },
            { data: 'min2' },
            { data: 'max2' },
            { data: 'characterization' },
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

    datatable.column(1).visible(false);
    datatable.column(6).visible(false);
}

function GetListCriterion() {
    // Obtener los datos de los formularios desde el controlador
    $.get('/Form/GetListCriterion', function (data) {
        console.log(data);
        // Limpiar el select
        $('.Criterion').empty();

        // Llenar el select con los datos obtenidos
        $.each(data, function (index, item) {
            $('.Criterion').append($('<option>', {
                value: item.id,
                text: item.criterionText
            }));
        });
    });
}


$(document).ready(function () {
    GetListCriterion();
    GetAllCharacterization();
});