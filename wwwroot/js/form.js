var datatable;
function GetAllForm() {
    _Forms = [];

    datatable = $('#tblForm').DataTable({
        dom: 'fBrtlp',
        responsive: true,
        orderCellsTop: true,
        select: true,
        deferRender: true,
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
            url: '/Form/GetAllForm',
            dataSrc: function (data) {
                return _Forms = data;

            }
        },

        columns: [

            { data: 'id' },
            { data: 'nameForm' },
            { data: 'menu' },
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

$(document).ready(function () {
    GetAllForm();
});