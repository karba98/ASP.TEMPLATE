document.onload = RefreshOfertas();

var span = document.getElementsByClassName("close")[0];

// When the user clicks on the button, open the modal
//btn.onclick = function() {
//  modal.style.display = "block";
//}

// When the user clicks on <span> (x), dont close the modal
span.onclick = function () {}


// When the user clicks anywhere outside of the modal, dont close it
window.onclick = function (event) {
    if (event.target == modal) {}
}


function RefreshOfertas() {
    $.get('/Empleo/RefreshRSSs',null,
        function (data) {
            if (data == 1) {
                window.location.replace("/Empleo/ManageEmpleo?alert_message=¡Tienes " + data + " nueva oferta disponibles!&alert_class=success");

            }
            else if (data>1) {
                window.location.replace("/Empleo/ManageEmpleo?alert_message=¡Tienes "+data+" nuevas ofertas disponibles!&alert_class=success");
                
            } else {
                window.location.replace("/Empleo/ManageEmpleo?alert_message=No se encontraron nuevas ofertas&alert_class=secondary");
            }

        }
    );

}