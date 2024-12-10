window.addEventListener('load', function () {
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');
    const footer = document.getElementById('footer');

    // Si la pantalla es móvil, ocultamos el sidebar
    if (window.innerWidth <= 768) {
        sidebar.classList.add('hide');
        content.style.marginLeft = '0';
        footer.style.marginLeft = '0';
    } else {
        sidebar.classList.remove('hide');
        content.style.marginLeft = '250px';  // Ajustar según el tamaño del sidebar
        footer.style.marginLeft = '250px';
    }
});

document.getElementById('sidebarToggle').addEventListener('click', function () {
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');
    const footer = document.getElementById('footer');

    // Si la pantalla es móvil (<= 768px)
    if (window.innerWidth <= 768) {
        if (sidebar.classList.contains('fullscreen-sidebar')) {
            // Si está en pantalla completa, lo ocultamos
            sidebar.classList.remove('fullscreen-sidebar');
            sidebar.classList.add('hide');
            content.style.display = 'block';  // Mostramos el contenido
            footer.style.display = 'block';   // Mostramos el footer
        } else if (sidebar.classList.contains('hide')) {
            // Si está oculto, lo mostramos en pantalla completa
            sidebar.classList.remove('hide');
            sidebar.classList.add('fullscreen-sidebar');
            content.style.display = 'none';   // Ocultamos el contenido
            footer.style.display = 'none';    // Ocultamos el footer
        }
    } else {
        // En pantallas grandes (> 768px), alternamos entre ocultar/mostrar con la clase 'hide'
        sidebar.classList.toggle('hide');
        if (sidebar.classList.contains('hide')) {
            content.style.marginLeft = '0';
            footer.style.marginLeft = '0';
        } else {
            content.style.marginLeft = '250px';
            footer.style.marginLeft = '250px';
        }
    }
});

document.getElementById('userToggle').addEventListener('click', function (event) {
    event.preventDefault(); 
    const userMenu = document.getElementById('userMenu');
    userMenu.style.display = userMenu.style.display === 'block' ? 'none' : 'block';
});

document.getElementById('companyToggle').addEventListener('click', function (event) {
    event.preventDefault();
    const submenu = document.getElementById('companySubmenu');
    submenu.style.display = submenu.style.display === 'block' ? 'none' : 'block';
});

function CloseSession() {
    $.ajax({
        type: "POST",
        url: "/Login/Close",
        success: function (data) {
            window.location.href = "/Home";
        }
    });
}