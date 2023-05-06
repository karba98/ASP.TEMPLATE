
function Publicar(pressedButton, id) {
    let row = $(pressedButton).parent().parent();
    $(pressedButton).html(
        `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`
    );
    let tr = $(pressedButton).parent().parent();
    let td_actions = tr.children('.td_actions');
    let td_fecha = tr.children('.td_fecha');
    let
    td_action = tr.children('.td_action');
    let td_action_button = td_action.children('.button');
    $.get('/Empleo/PublicarEnWeb', {"id":id},
        function (data) {
            if (data != null) {

                var myurl = data.urlVigi;

                //DA EL OK
                row.removeClass().addClass("trpub");
                row.children('.td_action').children('button').removeAttr('onclick')
                row.children('.td_modo').html('P')
                row.children('.td_action').children('button').removeClass('button_publish');
                $(pressedButton).css('background-color', 'green !important');
                $(pressedButton).html('OK');

                td_fecha.html(data.fechaString)

                
                //row.children('.td_action').html('');
            } else {

                row.children('.td_action').children('button').removeAttr('onclick')
                row.children('.td_modo').attr('P')
                row.children('.td_action').children('button').removeClass('button_publish');
                $(pressedButton).css('background-color', 'red !important');
                $(pressedButton).html('ERROR');

            }
            setTimeout(function () {
                $(pressedButton).html('ENVIAR');


                td_action_button.attr("onclick", "sendToICARO(this)");
                //"sendToICARO(this)"
                td_actions.children()[0].href = "/Empleo/Delete?id=" + data.id;
                td_actions.children()[1].href = "/Empleo/EditEmpleo?Modo=PU&id=" + data.id;
                td_actions.children()[2].href = "/Empleo/Oferta?provincia=" + data.provincia + "&fecha=" + data.fechaString;
                td_actions.children()[2].innerHTML = "<img src=\"/assets/webfonts/info-square.svg\" alt=\"Bootstrap\" width=\"23\" height=\"23\">";

                row.children('.td_urlvigi').html(myurl);
                //HABLITAMOS EL BOTON PARA ENVIAR A ICARO
                $(pressedButton).css('background-color', '#ff8d1e !important');
            }, 5000);

        }
    );

}