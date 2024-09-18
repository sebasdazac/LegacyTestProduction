Chart.register(ChartDataLabels);


document.getElementById('sidebarToggle').addEventListener('click', function () {
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');
    const footer = document.getElementById('footer');

    // Alternar la visibilidad del sidebar
    sidebar.classList.toggle('hide');

    // Ajustar el ancho del contenido
    if (sidebar.classList.contains('hide')) {
        content.style.marginLeft = '0'; // Expandir el contenido al 100% del ancho
        footer.style.marginLeft = '0'; // Expandir el contenido al 100% del ancho
    } else {
        content.style.marginLeft = '250px'; // Dejar espacio para el sidebar
        footer.style.marginLeft = '250px'; // Dejar espacio para el sidebar
    }
});


// Mostrar/Ocultar menú de usuario al hacer clic en "Administrador"
document.getElementById('userToggle').addEventListener('click', function (event) {
    event.preventDefault(); // Prevenir el comportamiento por defecto del enlace
    const userMenu = document.getElementById('userMenu');
    userMenu.style.display = userMenu.style.display === 'block' ? 'none' : 'block';
});

// Mostrar/Ocultar submenú al hacer clic en "Servicios"
//document.getElementById('servicesToggle').addEventListener('click', function (event) {
//    event.preventDefault(); // Prevenir el comportamiento por defecto del enlace
//    const submenu = document.getElementById('servicesSubmenu');
//    submenu.style.display = submenu.style.display === 'block' ? 'none' : 'block';
//});


function CloseSession() {
    $.ajax({
        type: "POST",
        url: "/Login/Close",
        success: function (data) {
            window.location.href = "/Home";
        }
    });
}
