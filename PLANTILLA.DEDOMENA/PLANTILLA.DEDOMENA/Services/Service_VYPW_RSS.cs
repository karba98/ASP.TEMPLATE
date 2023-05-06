using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Clases;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;

namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_VYPW_RSS
    {
        /// <summary>
        /// Service Gestor de Acciones para genrar Post's Obtenidos de 
        /// ARSEPRI y BBDD para RSS 
        /// </summary>
        readonly RepositoryEmpleo _repo;
        readonly DataManager _datamanager;
        public Service_VYPW_RSS(RepositoryEmpleo _repo, DataManager _datamanager)
        {
            this._datamanager = _datamanager;
            this._repo = _repo;
        }
        /// <summary>
        /// Convierte todas las ofertas de BBDD en una lista de Posts para RSS
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetOfertasAsPosts()
        {
            try
            {
                List<Empleo> list1 = _repo.OfertasTop20();
                List<Post> list = new List<Post>();
                foreach (Empleo e in list1)
                {
                    if (!e.FechaString.Contains("TT"))
                    {
                        if(e.FechaString.LastIndexOf(' ') == e.FechaString.Length - 1)
                        {
                            e.FechaString = e.FechaString.Remove(e.FechaString.LastIndexOf(' '));
                        }
                        DateTime fecha;
                        fecha = DateTime.ParseExact(e.FechaString, "dd/MM/yyyy HH:mm:ss tt", null);

                        string p = _datamanager.GetProvinciaRoute(e.Provincia);
                        Post post = new Post(
                            e.Titulo,
                            "Nueva Oferta de empleo",
                            e.Descripcion,
                            "empleo",
                            "fjcastro",
                            "https://vigilanciayproteccion.website/Empleo/Oferta?provincia=" + p + "&fecha=" + System.Net.WebUtility.UrlEncode(e.FechaString),
                            fecha
                        );
                        list.Add(post);
                    }
                }
                return await Task.FromResult<IEnumerable<Post>>(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
        public async Task<IEnumerable<Post>> GetNoticiasAsPosts()
        {
            try
            {

                RSSReader r = new RSSReader();
                List<Post> postlist = r.ParseRSSdotnet("https://www.abc.es/rss/feeds/abc_EspanaEspana.xml");
                List<Post> postlist1 = r.ParseRSSdotnet("https://h50policia.es/feed/");
                List<Post> postlist2 = r.ParseRSSdotnet("https://www.seguritecnia.es/feed/");
                List<Post> postlist3 = r.ParseRSSdotnet("https://www.telemadrid.es/noticias/feed");
                List<Post> postlist4 = r.ParseRSSdotnet("https://feeds.elpais.com/mrss-s/pages/ep/site/elpais.com/section/espana/portada");
                List<Post> postlist5 = r.ParseRSSdotnet("https://okdiario.com/feed");

                List<Post> posts = new List<Post>();

                if(postlist!=null && postlist.Count>0) posts.AddRange(postlist);
                if(postlist1!=null && postlist1.Count>0) posts.AddRange(postlist1);
                if(postlist2!=null && postlist2.Count>0) posts.AddRange(postlist2);
                if(postlist3!=null && postlist3.Count>0) posts.AddRange(postlist3);
                if(postlist4!=null && postlist4.Count>0) posts.AddRange(postlist4);
                if(postlist5!=null && postlist5.Count>0) posts.AddRange(postlist5);

                posts = r.Filter(posts);
                return posts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

    }
}
