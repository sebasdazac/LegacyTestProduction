var estadoEtapa = document.getElementById('estadoEtapa');
var characterization3 = document.getElementById('characterization3');

$.ajax({
    type: "POST",
    url: "/CharacterizationByCompany/GetChart3",
    success: function (data) {    
      
        updateProgress(estadoEtapa, data.value1);
        characterization3.innerText = data.characterization;    
        renderAccordion(data, 'effectsAndRecommendationsAlert3');
    }   
});

