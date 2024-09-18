let datatable;
function Edit(idplan) {
    let plan = _plans.find(x => x.idplan == idplan);
    $('#ModalplanLabel').text('Editar plan');
    $('#btnSaveplan').text('Actualizar');
    $('#Idplan').val(plan.idplan);
    $('#Cardplan').val(plan.cardplan).prop("readonly", true);
    $('#Apellidos').val(plan.apellidos);
    $('#Nombres').val(plan.nombres);
    $("#regEstado").removeAttr("hidden");
    $('#Estado').val(plan.estado);
    $('#Modalplan').modal('show');

}

function Delete(idplan) {
    Swal.fire({
        title: 'Está Seguro de eliminar?',
        text: 'Este registro no se podrá recuperar',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Borrar',
        cancelButtonText: 'Cancelar',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: "plans/Delete?idplan=" + idplan,
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                    }
                    else {
                        Swal.fire({ text: data.message });
                    }
                }
            })
        }
    })
}

function GetAllPlan() {
    _plans = [];

    datatable = $('#tblplan').DataTable({
        dom: 'fBrtlp',
        responsive: true,
        orderCellsTop: true,
        select: true,
        language: {
            url: "//cdn.datatables.net/plug-ins/1.11.4/i18n/es-mx.json"
        },
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, ':visible']
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: [0, ':visible']
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: [0, ':visible']
                }
            },
            'colvis',
            'searchBuilder'
        ],
        ajax: {
            type: 'GET',
            url: '/Form/GetAllPlan',
            dataSrc: function (data) {
                return _plans = data;
            }
        },

        columns: [
            { data: 'id' },
            { data: 'namePlan' },
            { data: 'price' },
            { data: 'isActive' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class='text-center'> 
                        <button class="btn btn-outline-secondary p-1" style="cursor: pointer" onclick="Edit(${data})"><i class='bx bx-edit'></i></button>                              
                        <button class="btn btn-primary p-1" style="cursor: pointer" onclick="Edit(${data})"><i class='bx bx-trash' ></i></button>                  
                  </div>`;
                }
            }
        ]
    });
}

function SaveOrUpdate(event) {
    event.preventDefault();
    let plan = $('#formPlan').serialize();
    $.ajax({
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/Form/SaveOrUpdatePlan",
        data: plan,
        success: function (data) {
            if (!data.data.success) {
                Swal.fire({ text: data.data.errorMessage });
            } else {
                datatable.ajax.reload();
            }
        }
    });
    return false;
}

$(document).ready(function () {
    GetAllPlan();
});