﻿$(document).ready(function () {
    let counter = 5;
    let timerInterval = setInterval(() => {
        var textCounter = document.getElementById("textCounter");
        textCounter.innerHTML = "Don't worry, i can send you back in " + counter + " seconds.";
        counter--;
    }, 1000);
    

    setTimeout(function () {
        clearInterval(timerInterval);
        var code = document.getElementsByClassName("error");
        if (code[0].innerHTML === "401") {
            window.location.replace("../Login/");
        }
        else {
            window.location.replace("../Admin/");
        }
    }, 5500);

});