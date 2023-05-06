//Load carrousel data
$.get('/Home/GetData', null,
    function (data) {
        /*console.log(data);*/

        let base_url = window.location.origin;
        //NOTICIA
        $("#noticia_img").attr("src", data.noticia.img);
        $("#noticia_titulo").text(data.noticia.titulo);
        $("#noticia_fecha").text(data.noticia.fechaPub);
        let params_noticia = {
            title: data.noticia.titleUrl,
            fecha: data.noticia.fechaPubUrl
        }
        let url_noticia = base_url + "/Home/DetallesNoticia?" + jQuery.param(params_noticia);
        $("#noticia_url").prop("href", url_noticia);

        //OFERTA
        $("#oferta_img").attr("src", "/assets/images/empleo/" + data.oferta.categoria + ".jpg");
        $("#oferta_titulo").text(data.oferta.titulo);
        let params_oferta = {
            provincia: data.oferta.provincia,
            fecha: data.oferta.fechaString
        }
        let url_oferta = base_url + "/Empleo/Oferta?" + jQuery.param(params_oferta);
        $("#oferta_url").prop("href", url_oferta);

        //OTRA NOTICIA
        $("#otranoticia_img").attr("src", data.otra_noticia.img);
        $("#otranoticia_titulo").text(data.otra_noticia.titulo);
        $("#otranoticia_fecha").text(data.otra_noticia.fechaPub);
        let params_otranoticia = {
            title: data.otra_noticia.titleUrl,
            fecha: data.otra_noticia.fechaPubUrl
        }
        let url_otranoticia = base_url + "/Home/DetallesNoticia?" + jQuery.param(params_otranoticia);
        $("#otranoticia_url").prop("href", url_otranoticia);

        //CURSO
        $("#curso_img").attr("src", data.curso.img);
        $("#curso_titulo").text(data.curso.titulo);
        $("#curso_fecha").text(data.curso.fechaPub);
        let params_curso = {
            title: data.curso.titleUrl,
            fecha: data.curso.fechaPubString
        }
        let url_curso = base_url + "/Home/DetallesCurso?" + jQuery.param(params_curso);
        $("#curso_url").prop("href", url_curso);
        $(".spin_carousel").each(function () {
            var current_element = $(this);
            current_element.remove();
        });
    }
);