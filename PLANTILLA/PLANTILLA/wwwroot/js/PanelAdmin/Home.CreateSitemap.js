let botonsitemap = $("#btnsitemap");

botonsitemap.click(function () {
    let img = $("#img_sitemap");
    $("#img_sitemap").remove();
    $(botonsitemap).html(
        `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`
    );
    $.get('/Home/CreateSitemap', { },
        function (data) {
            if (data != null) {

                $(botonsitemap).css('background-color', 'green !important');
                $(botonsitemap).html('OK');
            } else {


                $(botonsitemap).css('background-color', 'red !important');
                $(botonsitemap).html('KO');

            }

            setTimeout(function () {
                //#ff8d1e
                $(botonsitemap).css('background-color', '#ff8d1e !important');
                $(botonsitemap).html('');
                botonsitemap.append(img);

                
            }, 5000);

        }
    );
})