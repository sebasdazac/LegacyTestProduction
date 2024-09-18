var datatable;
function GetAllAnswer() {
    _Answers = [];

    datatable = $('#tblAnswer').DataTable({
        dom: 'fBrtlp',
        ajax: {
            type: 'GET',
            url: '/Form/GetAllAnswer',
            dataSrc: function (data) {
                return data;
            }
        },
        columns: [
            { data: 'id' },
            { data: 'idQuestion' },
            { data: 'answerText' },
            { data: 'value' },
            { data: 'isActive' },
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

}



function GetListQuestionParent() {
    $.get('/Form/GetListQuestionParent', function (data) {
        // Limpiar el select
        $('#IdQuestion').empty();

        // Llenar el select con los IDs obtenidos
        $.each(data, function (index, id) {
            $('#IdQuestion').append($('<option>', {
                value: id,
                text: id
            }));
        });
    });
}

$(document).ready(function () {
    GetListQuestionParent();
    GetAllAnswer();   
});