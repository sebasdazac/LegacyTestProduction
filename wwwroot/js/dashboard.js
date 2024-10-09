Chart.register(ChartDataLabels);
document.getElementById('sidebarToggle').addEventListener('click', function () {
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');
    const footer = document.getElementById('footer');  
    sidebar.classList.toggle('hide');

   
    if (sidebar.classList.contains('hide')) {
        content.style.marginLeft = '0'; 
        footer.style.marginLeft = '0';
    } else {
        content.style.marginLeft = '250px'; 
        footer.style.marginLeft = '250px'; 
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