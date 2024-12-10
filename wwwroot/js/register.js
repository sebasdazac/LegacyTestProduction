function RegisterPerson() {
    var data = {
        Name: document.getElementById('name').value.toUpperCase(),
        Surname: document.getElementById('surname').value.toUpperCase(),
        Email: document.getElementById('email').value,
        Pswd: document.getElementById('inputPassword').value,
        State: 'Inactivo',
    };

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Register/RegisterPerson', true);
    xhr.setRequestHeader('Content-Type', 'application/json');

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            Swal.close(); // Cierra el mensaje de carga
            document.getElementById('registerForm').reset();
            if (xhr.status === 200) {
                Swal.fire({
                    icon: 'success',
                    title: 'Registro exitoso',
                    text: xhr.responseText
                }).then(() => {
                    window.location.href = "/Login/Index";
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: xhr.responseText
                });
            }
        }
    };

    // Mostrar mensaje de carga
    Swal.fire({
        title: 'Registrando en LegacyTest',
        text: 'Por favor, espera...',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    xhr.send(JSON.stringify(data));
}

document.getElementById('btnRegister').addEventListener('click', function () {
    var form = document.getElementById('registerForm');
    var password = document.getElementById('inputPassword').value;
    var confirmPassword = document.getElementById('inputConfirmPassword').value;

    if (form.checkValidity() && validatePassword(password, confirmPassword)) {
        RegisterPerson();
    } else {
        form.reportValidity();
    }
});

function validatePassword(password, confirmPassword) {
    var passwordCriteria = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s])[^\s]{8,}$/;
    if (password !== confirmPassword) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Las contraseñas no coinciden'
        });
        return false;
    }
    if (!passwordCriteria.test(password)) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y caracteres especiales'
        });
        return false;
    }
    return true;
}

document.getElementById('name').addEventListener('input', function (event) {
    event.target.value = event.target.value.toUpperCase();
});

document.getElementById('surname').addEventListener('input', function (event) {
    event.target.value = event.target.value.toUpperCase();
});
