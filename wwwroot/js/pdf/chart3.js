var estadoEtapa = document.getElementById('estadoEtapa');
var characterization3 = document.getElementById('characterization3');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart3?param=" + param,
    success: function (data) {    
      
        updateProgress(estadoEtapa, data.value1);
        characterization3.innerText = data.characterization;            
    },
    complete: function () {
        window.JSREPORT_READY_TO_START = true
    }
});

