
var screen = true;
//screen - control pantlla pequeña o grande para tabla (evita renderizar todas las filas en cada resize)
$(window).on('resize', function () {
    let width = $(this).width();
    if (width <= 990 && screen == true) {
        var rows = $('#datos tr');
        rows.each(function () {
            swapElements(this, 0, 2);
        });
        $('#datos tbody tr td:first-child a').click(function () {
            var $this = $(this).parent();
            //Ponemos todas las demas filas con texto normal sin subrayado
            var style_fontweight = $this.css('fontWeight');
            if (style_fontweight == 700) { //700 = bold
                //si el elemento estaba expandido lo cerramos

                //color del titulo
                var title = $(this).parent();

                title.css({ "background-color": "" });
                //tr style="background-color:white !important;color:black !important"

                $(this).parent().siblings().css({ 'display': 'none', 'transform': 'translateY(-9999px)' });
                this.parentElement.style.fontWeight = "normal";
                this.parentElement.style.textDecoration = "none";

                let r = $(this).parent().parent();
                r.attr('style', '');
            }
            else {
                var rows = $('#datos tbody tr');
                rows.each(function () {
                    this.children[0].style.fontWeight = "normal"
                    this.children[0].style.textDecoration = "none"
                });
                //Abrimos bloque de datos
                var elem = $(this).parent().siblings();

                //color del titulo
                var title = $(this).parent();
                title.css({ "background-color": "#ff8d1e" });

                elem.css({ 'display': 'inline-block' });

                this.parentElement.style.fontWeight = "bold";
                this.parentElement.style.textDecoration = "underline";

                let r = $(this).parent().parent();
                r.attr('style', 'background-color:white !important;color:black !important');


                setTimeout(function () {
                    $this.siblings().css('transform', 'translateY(0)');
                }, 0);
                //Cerramos el bloque de todas las filas si hubieramos abierto uno anteriormente
                //color del titulo
                $('#datos tbody tr td:first-child').not($(this).parent()).css({ 'background-color': '' });

                $('#datos tbody tr td:first-child').not($(this).parent()).siblings().css({ 'display': 'none', 'transform': 'translateY(-9999px)' });
                $('#datos tbody tr td:last-child').css({ 'display': 'none' });
                $('#datos tbody tr').not($(this).parent().parent()).not('[style*="display: none"]').attr('style', '');
            }

        });
        screen = false;
    } else if (width > 990 && screen == false) {
        var rows = $('#datos tr');
        rows.each(function () {
            swapElements(this, 0, 2);
        });

        //quitamos funcion click si la pantalla es > 990px
        $("#datos tbody tr td a").unbind("click");
        $("#datos tbody tr td").css({ 'font-weight': 'normal', 'text-decoration': 'none' });
        //limpiamos estilos en tds
        $('#datos tr td').siblings().css({ 'display': '', 'transform': '', 'background-color': '' });
        $('#datos tr').not('[style*="display: none"]').attr('style', '');
        screen = true;
    }

}).resize();

function swapElements(parent, elemA, elemB) {
    //validamos elementos
    if (!parent || parent.constructor.toString().search('HTML') === -1) return;
    var children = parent.children;
    if (typeof elemA !== 'number' || typeof elemB !== 'number' || elemA === elemB || !children[elemA] || !children[elemB]) return;

    elemB = elemA < elemB ? elemB-- : elemB;
    var childNumb = children.length - 1;

    //swap
    var a = parent.removeChild(children[elemA]);
    var b = parent.removeChild(children[elemB]);
    append(elemB, a);
    append(elemA, b);

    function append(a, b) {
        childNumb === a ? parent.appendChild(b) : parent.insertBefore(b, children[a]);
    }
}