document.addEventListener('DOMContentLoaded', function () {
    // Llama al controlador para obtener la información de la empresa
    $.ajax({
        url: '/Company/GetOrganizationInfo',
        type: 'GET',
        success: function (data) {
            if (data) {
                // Asignar los valores recibidos a los inputs del formulario
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


    // Actualizar la información de la compañía
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
            url: '/Company/UpdateOrganizationInfo',
            type: 'POST',
            data: {
                businessName: businessName,
                typeReg: typeReg,
                commercialReg: commercialReg
            },
            success: function (response) {
                // Recargar la tabla para mostrar el nuevo miembro
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





    // Configurar DataTable
    $('#peopleTable').DataTable({
        "ajax": {
            "url": "/Company/GetPeople",
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



    // Capturar el valor del input y enviarlo por AJAX al activar el plan gratuito
    document.getElementById('activatePlanButton').addEventListener('click', function () {
        var tokenValue = document.getElementById('token').value;

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
            url: '/Company/ActivatePlanFree',
            type: 'POST',
            data: { token: tokenValue },
            success: function (response) {
                // Recargar la tabla para mostrar el nuevo miembro
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
                    text: 'Error al activar el plan.'
                });
            }
        });
    });

    // Manejar el envío del formulario en el modal
    // Manejar el envío del formulario en el modal
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
            url: '/Company/InviteColaborator',
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

                    // Limpiar el formulario después de enviar los datos
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
            const response = await fetch('/Company/ValidateInvitation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ token: token })
            });

            const result = await response.json();

            document.getElementById('addCompanyForm').reset(); // Limpia el formulario

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
                        // Realiza una solicitud POST para cerrar sesión
                        await fetch('/Login/Close', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        });
                        window.location.href = '/home';
                    } else {
                        location.reload(); // Recarga la página si el usuario decide permanecer
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
            document.getElementById('addCompanyForm').reset(); // Limpia el formulario

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al procesar la solicitud.',
            });
        }
    });



});
