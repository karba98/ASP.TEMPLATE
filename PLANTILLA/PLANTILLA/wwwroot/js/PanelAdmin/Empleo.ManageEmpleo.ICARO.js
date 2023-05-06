
function sendToICARO(pressedButton) {
    $(pressedButton).prop("disabled", true);

    $(".button_send").each(function () {
        $(this).prop("disabled", true);
    });
    // add spinner to button
    $(pressedButton).html(
        `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`
    );
    var row = $(pressedButton).parent().parent()
    var url = row.children('.td_urlvigi').text();
    var provincia = row.children('.td_provincia').text().trim();
    var titulo = row.children('.td_titulo').text().trim();

    $.get('/Empleo/PostOferta', {"url": url, "provincia": provincia, "titulo": titulo },
        function (data) {
            //$('#myModal').css("display", "block")
            if (data == true) {
                //$('#modaltext').text("Se ha enviado la oferta a RRSS con exito")
                $(pressedButton).css('background-color', 'green');
                $(pressedButton).html('OK');
                
            } else {
                $(pressedButton).html('ERROR');
                //boton rojo
                $(pressedButton).css('background-color', 'red');
            }

            //espera 5 segundos
            setTimeout(function () {
                $(pressedButton).html('ENVIAR');
                //vuelvo boton a su estado primario
                $(pressedButton).css('background-color', '#ff8d1e');
                $(pressedButton).prop("disabled", false);

                $(".button_send").each(function () {
                    $(this).prop("disabled", false);
                });
            }, 5000);
            

        }
    );
    
}