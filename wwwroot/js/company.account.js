document.addEventListener('DOMContentLoaded', function () {
    // Cargar datos de perfil al cargar la página
    $.ajax({
        url: '/Person/GetProfile',
        type: 'GET',
        success: function (data) {
            document.getElementById('inputFirstName').value = data.firstName;
            document.getElementById('inputLastName').value = data.lastName;
            document.getElementById('inputEmailAddress').value = data.email;
        },
        error: function () {
            Swal.fire('Error', 'Hubo un problema al cargar tus datos.', 'error');
        }
    });

    document.getElementById('btnDeleteAccount').addEventListener('click', function () {
        Swal.fire({
            title: '¿Estás seguro?',
            text: "Esta acción no se puede deshacer!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Person/DeleteAccount',
                    type: 'DELETE',
                    success: function () {
                        Swal.fire('Eliminado!', 'Tu cuenta ha sido eliminada.', 'success')
                            .then(() => {
                                window.location.href = '/';
                            });
                    },
                    error: function () {
                        Swal.fire('Error', 'Hubo un problema al eliminar tu cuenta.', 'error');
                    }
                });
            }
        });
    });

    document.getElementById('formUpdateProfile').addEventListener('submit', function (e) {
        e.preventDefault();
        var formData = {
            firstName: document.getElementById('inputFirstName').value,
            lastName: document.getElementById('inputLastName').value,
            email: document.getElementById('inputEmailAddress').value
        };

        $.ajax({
            url: '/Person/UpdateProfile',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function () {
                Swal.fire('Actualizado', 'Tu perfil ha sido actualizado.', 'success');
            },
            error: function () {
                Swal.fire('Error', 'Hubo un problema al actualizar tu perfil.', 'error');
            }
        });
    });

    document.getElementById('formChangePassword').addEventListener('submit', function (e) {
        e.preventDefault();

        var formData = {
            currentPassword: document.getElementById('currentPassword').value,
            newPassword: document.getElementById('newPassword').value,
            confirmPassword: document.getElementById('confirmPassword').value
        };


        var passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;

        if (!passwordRegex.test(formData.newPassword)) {
            Swal.fire({
                icon: 'warning',
                title: 'Contraseña débil',
                text: 'La nueva contraseña debe tener al menos 8 caracteres, incluir una mayúscula, una minúscula, un número y un carácter especial.'
            });
            return;
        }

        if (formData.newPassword !== formData.confirmPassword) {
            Swal.fire({
                icon: 'warning',
                title: 'Error de coincidencia',
                text: 'Las contraseñas no coinciden.'
            });
            return;
        }


        $.ajax({
            url: '/Person/ChangePassword',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            beforeSend: function () {
                Swal.fire({
                    title: 'Cambiando contraseña...',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Cambiado',
                    text: 'Tu contraseña ha sido cambiada.'
                }).then(function () {
                    document.getElementById("formChangePassword").reset();
                });
            },
            error: function (xhr) {
                var errorMessage = 'Hubo un problema al cambiar tu contraseña.';
                if (xhr.responseText) {
                    errorMessage = xhr.responseText;
                }
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: errorMessage
                });
            }
        });
    });

});
