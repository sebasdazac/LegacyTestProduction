$(document).ready(function () {
    document.getElementById('formLogin').reset();
});

function SessionForm(event) {

    if (!formLogin.checkValidity()) {
        console.log('Nop');
    }
    else {
        event.preventDefault();
        var email = document.getElementById("email").value;
        var pswd = document.getElementById("pswd").value;

        $.ajax({
            url: "/Login/Session",
            type: 'POST',
            data: { email: email, pswd: pswd },
            success: function (data) {
                if (data.success) {
                 
                    document.getElementById('formLogin').reset();
                    window.location.href = "/Dashboard";
                } else {
                    document.getElementById('formLogin').reset();   
                
                    Swal.fire({ text: data.errorMessage });
                }
            },
            beforeSend: function () {             
                Swal.fire({
                    icon: 'info',
                    text: 'Un momento por favor, estamos consultando la información',
                    showCancelButton: false,
                    showConfirmButton: false
                });
            }
        });
        return false;
    }
}
