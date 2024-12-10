document.addEventListener('DOMContentLoaded', function () {   
    $.ajax({
        url: '/CompanyInfo/GetOrganizationInfo',
        type: 'GET',
        success: function (data) {
            if (data) {              
                document.getElementById('BusinessName').value = data.businessName || '';
                document.getElementById('TypeReg').value = data.typeReg || '';
                document.getElementById('CommercialReg').value = data.commercialReg || '';
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error al obtener la información de la empresa.'
            });
        }
    });

    document.getElementById('updateOrganizationButton').addEventListener('click', function () {
        var form = document.getElementById('companyForm');

        if (form.checkValidity() === false) {
            form.reportValidity();
            return;
        }

        var businessName = document.getElementById('BusinessName').value;
        var typeReg = document.getElementById('TypeReg').value;
        var commercialReg = document.getElementById('CommercialReg').value;

        Swal.fire({
            title: 'Enviando...',
            text: 'Estamos procesando tu solicitud.',
            icon: 'info',
            showConfirmButton: false,
            allowOutsideClick: false,
            willOpen: () => {
                Swal.showLoading();
            }
        });

        $.ajax({
            url: '/CompanyInfo/UpdateOrganizationInfo',
            type: 'POST',
            data: {
                businessName: businessName,
                typeReg: typeReg,
                commercialReg: commercialReg
            },
            success: function (response) {               
                $('#peopleTable').DataTable().ajax.reload();
                if (response.success) {                 

                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: response.message
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al actualizar la información de la empresa.'
                });
            }
        });
    });

    $('#peopleTable').DataTable({
        "ajax": {
            "url": "/CompanyInfo/GetPeople",
            "type": "GET",
            "dataSrc": ""
        },
        "columns": [
            { "data": "name" },
            { "data": "email" },
            { "data": "role" },
            { "data": "state" }
        ],
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/2.1.0/i18n/es-MX.json",
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Mostrando 0 a 0 de 0 registros",
            "infoFiltered": "(Filtrado de _MAX_ registros totales)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ registros",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron resultados",
            "paginate": {
                "first": "<<",
                "last": ">>",
                "next": ">",
                "previous": "<"
            },
            "aria": {
                "sortAscending": ": activar para ordenar la columna en orden ascendente",
                "sortDescending": ": activar para ordenar la columna en orden descendente"
            }
        }
    });


  
    document.getElementById('submitAddMemberForm').addEventListener('click', function () {
        var form = document.getElementById('addMemberForm');

        if (form.checkValidity() === false) {
            form.reportValidity();
            return;
        }

        var name = document.getElementById('memberName').value;
        var email = document.getElementById('memberEmail').value;

        Swal.fire({
            title: 'Enviando...',
            text: 'Estamos procesando tu solicitud.',
            icon: 'info',
            showConfirmButton: false,
            allowOutsideClick: false,
            willOpen: () => {
                Swal.showLoading();
            }
        });

        $.ajax({
            url: '/CompanyInfo/InviteColaborator',
            type: 'POST',
            data: {
                name: name,
                email: email
            },
            success: function (response) {
                Swal.close();
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: response.message
                    });
                    $('#addMemberModal').modal('hide');
                   
                    form.reset();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al enviar la invitación.'
                });
            }
        });
    });   

    document.getElementById('submitAddCompanyForm').addEventListener('click', async function () {
        const token = document.getElementById('tokenInvitation').value;

        if (!token) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Por favor ingresa el código de invitación.',
            });
            return;
        }

        try {
            const response = await fetch('/CompanyInfo/ValidateInvitation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ token: token })
            });

            const result = await response.json();

            document.getElementById('addCompanyForm').reset(); 

            if (result.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: result.message,
                    showCancelButton: true,
                    confirmButtonText: 'Cerrar sesión',
                    cancelButtonText: 'Permanecer en la página'
                }).then(async (response) => {
                    if (response.isConfirmed) {                       
                        await fetch('/Login/Close', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        });
                        window.location.href = '/home';
                    } else {
                        location.reload(); 
                    }
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: result.message,
                });
            }
        } catch (error) {
            document.getElementById('addCompanyForm').reset(); 

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al procesar la solicitud.',
            });
        }
    });
});