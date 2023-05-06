using System.Collections.Generic;

namespace PLANTILLA.Models
{
    public class DataContainer
    {
        //ViewBag.Last3Ofertas = ofertas ?? null;
        //ViewBag.Noticias = noticias ?? null;
        //ViewBag.OtrasNoticias = otras_noticias ?? null;
        //ViewBag.Cursos = cursos ?? null;

        //ViewBag.Noticia = noticia ?? null;
        //ViewBag.OtraNoticia = otra_noticia ?? null;
        //ViewBag.Empleo = oferta ?? null;
        //ViewBag.Curso = curso ?? null;

        public List<Empleo> ofertas { get; set; }
        public List<Article> noticias { get; set; }
        public List<Article> otras_noticias { get; set; }
        public Article noticia { get; set; }
        public Article otra_noticia { get; set; }
        public Empleo oferta { get; set; }

    }
}
