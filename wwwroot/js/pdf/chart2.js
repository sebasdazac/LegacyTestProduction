var corporateEgo = document.getElementById('corporateEgo');
var familyEgo = document.getElementById('familyEgo');
var characterization2 = document.getElementById('characterization2');

var corporateEgoType = document.getElementById('corporateEgoType');
var familyEgoType = document.getElementById('familyEgoType');

window.onload = function () {
    $.ajax({
        type: "POST",
        url: "/CharacterizationByCompany/GetChart2?param="+param,
        success: function (data) {
            updateProgress(corporateEgo, data.value1);
            updateProgress(familyEgo, data.value2);
            characterization2.innerText = data.characterization;
            corporateEgoType.innerHTML = data.criterion1;
            familyEgoType.innerHTML = data.criterion2;           
        },
        complete: function () {
            window.JSREPORT_READY_TO_START = true
        }

    });

};