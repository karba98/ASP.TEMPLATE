$(document).ready(function () {
    var images = document.getElementsByClassName("webfeedsFeaturedVisual");
    var image = images[0];
    image.style.width = "100%";
    image.style.objectFit = "cover";

    var content = document.getElementById("description");
    if (content.childNodes[7].textContent.includes("Web de Ofertas de empleo")){
        content.childNodes[7].style.display = "none";

    }
    
});