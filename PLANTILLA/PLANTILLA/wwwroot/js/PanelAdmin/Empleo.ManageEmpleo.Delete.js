function deleteOferta(row, id) {
    $(row).parent().parent().css({ 'opacity': '50%' });
    $.get('/Empleo/DeleteOferta?id='+id, null,
        function (data) {
            if (data == true) {
                $(row).parent().parent().remove();
            } else {
                $(row).parent().parent().css({ 'opacity': '100%' });
            }

        }
    );
}

function deleteOfertaBR(row, id) {
    $(row).parent().parent().css({'opacity':'50%'});
    $.get('/Empleo/DeleteOfertaBR?id=' + id, null,
        function (data) {
            if (data == true) {
                $(row).parent().parent().remove();

            } else {
                $(row).parent().parent().css({ 'opacity': '100%' });
            }

        }
    );
}