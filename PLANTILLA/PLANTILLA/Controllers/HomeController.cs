using AspNetCore.SEOHelper.Sitemap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PLANTILLA.Filters;
using PLANTILLA.Helpers;
using PLANTILLA.Models;
using PLANTILLA.Services;

namespace PLANTILLA.Controllers
{
    public class HomeController : Controller
    {
        readonly Service_DEDOMENA _service;
        readonly DataManager datamanager;
        readonly ILogger<HomeController> _logger;
        readonly IWebHostEnvironment _env;

        public HomeController(
            Service_DEDOMENA _service,
            DataManager provm,
            ILogger<HomeController> _logegr,
            IWebHostEnvironment env
            )
        {
            this._logger = _logegr;
            this._service = _service;
            this.datamanager = provm;
            this._env = env;
        }
        public IActionResult TrabajandoEnEllo()
        {
            return View();
        }

        public IActionResult Index()
        {
            ViewBag.MetaTitle = "Inicio";
            ViewBag.Title = "Inicio";
            ViewBag.MetaDescription = "";
            ViewBag.MetaImg = "https://../assets/images/logo_card.png";
            return View();
        }
        public async Task<IActionResult> Noticias()
        {
            ViewBag.MetaTitle = "Noticias de vigilante seguridad";
            ViewBag.MetaDescription = "Noticias de vigilante seguridad";
            ViewBag.MetaImg = "https://../assets/images/logo_card.png";
            try
            {
                List<Article> noticias = await _service.CallApi<List<Article>>(
                    "Noticias/GetArticulos",
                    null,
                    new Dictionary<string, string>()
                    {
                        {"url","https://..//category/noticias/noticias-de-seguridad/feed/" }
                    },
                    null, "GET");
                return View(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return View(null);
            }
        }
        public async Task<IActionResult> DetallesNoticia(string title, string fecha)
        {
            try
            {
                List<Article> noticias = await _service.CallApi<List<Article>>(
                    "Noticias/GetArticulos",
                    null,
                    new Dictionary<string, string>()
                    {
                        {"url","https://..//category/noticias/noticias-de-seguridad/feed/" }
                    },
                    null, "GET"); 
                noticias = noticias.Take(3).ToList();
                ViewBag.Noticias = noticias;

            }
            catch (Exception ex) { _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException); }
            try
            {
                Article noticia = await _service.CallApi<Article>(
                    "Noticias/GetNoticia",
                    null,
                    new Dictionary<string, string>()
                    {
                        {"fecha",fecha },
                        {"titulo",title }
                    },
                    null, "GET");
                ViewBag.MetaTitle = noticia.Titulo;
                ViewBag.Title = noticia.Titulo;
                ViewBag.MetaDescription = noticia.Titulo + " "+ noticia.FechaPub;
                ViewBag.MetaImg = noticia.Img;
                return View(noticia);
            }
            catch (Exception ex)
            {
                ViewBag.MetaTitle = "Inicio";
                ViewBag.Title = "Noticias de vigilante seguridad";
                ViewBag.MetaDescription = "Noticias de vigilante seguridad";
                ViewBag.MetaImg = "https://../assets/images/logo_card.png";
                _logger.LogError("title: [" + title + "] fecha:[" + fecha + "]"
                    + ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);

                return View(null);
            }

        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult AcercaDe()
        {
            return View();
        }
        public IActionResult Politica()
        {
            return View();
        }
        public IActionResult PoliticaCookies()
        {
            return View();
        }


        public async Task<DataContainer> GetData()
        {
            DataContainer datos = await setDatos();
            return datos;
        }
        public async Task<DataContainer> setDatos()
        {
            try
            {

                DataContainer datos = null;
                string datacontainer = HttpContext.Session.GetString("dataContainer");
                if (datacontainer != null && datacontainer != "")
                {
                    datos = ToolKit.Deserialize<DataContainer>(datacontainer);
                }

                if (datos != null)
                {
                    return datos;
                }
                else
                {
                    datos = await _service.CallApi<DataContainer>(
                    "VYP/GetData",
                    null);
                    HttpContext.Session.SetString("dataContainer", ToolKit.Serialize(datos));
                    return datos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return null;
            }
        }

        //PARTIAL VIEW PANEL
        public async Task<IActionResult> PanelDatos()
        {
            Article noticia;
            Article otra_noticia;
            Empleo oferta;

            List<Empleo> ofertas = null;
            List<Article> noticias = null;
            List<Article> otras_noticias = null;
            try
            {

                DataContainer datos = null;
                string datacontainer = HttpContext.Session.GetString("dataContainer");
                if (datacontainer != null && datacontainer != "")
                {
                    datos = ToolKit.Deserialize<DataContainer>(datacontainer);
                }

                if (datos == null)
                {
                    datos = await _service.CallApi<DataContainer>(
                    "VYP/GetData",
                    null);


                    HttpContext.Session.SetString("dataContainer", ToolKit.Serialize(datos));
                }
                var myViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), 
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "DataContainer", datos } };
                myViewData.Model = datos;

                return new PartialViewResult()
                {
                    ViewName = "PanelDatos",
                    ViewData = myViewData,
                };
;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return new PartialViewResult()
                {
                    ViewName = "PanelDatos",
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    { }
                };
            }

            
        } 
        [AuthorizeUsers]
        public async Task<string> CreateSitemap()
        {
            try
            {

                var list = new List<SitemapNode>();
                string urlbase = $"{this.Request.Scheme}://www.{this.Request.Host}{this.Request.PathBase}/";
                //getdata
                DataContainer data = await _service.CallApi<DataContainer>(
                    "VYP/GetData",
                    null);
                //Main url
                list.Add(new SitemapNode
                {
                    LastModified = DateTime.Now,
                    Priority = 1.0,
                    Url = urlbase,
                    Frequency = SitemapFrequency.Hourly
                });
                //ofertas
                data.ofertas.Insert(0, data.oferta);
                list.Add(new SitemapNode
                {
                    LastModified = DateTime.Now,
                    Priority = 0.8,
                    Url = urlbase + "Empleo/ofertas/",
                    Frequency = SitemapFrequency.Hourly
                });
                List<string> provincias = datamanager.GetPorvicniasUrls();
                foreach (string prov in provincias)
                {
                    string url = urlbase + "Empleo/OfertasProvincia?provincia=" + prov+"/";
                    list.Add(new SitemapNode { LastModified = DateTime.Now, Priority = 0.8, Url = url, Frequency = SitemapFrequency.Hourly });
                }
                List<Empleo> ofertas = await _service.CallApi<List<Empleo>>("Empleo/ofertas", null);
                foreach (Empleo oferta in ofertas)
                {
                    var fecha = oferta.FechaString;
                    string url = urlbase + "Empleo/Oferta?provincia=" + oferta.Provincia + "&fecha=" + fecha;

                    string title = oferta.Titulo;
                    title = title.Replace(" ", "-");
                    title = Regex.Replace(title.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                    url += "&titulo=" + title + "/";

                    list.Add(new SitemapNode { LastModified = oferta.FechaPub, Priority = 0.8, Url = url, Frequency = SitemapFrequency.Hourly });
                }

                //noticias
                list.Add(new SitemapNode
                {
                    LastModified = DateTime.Now,
                    Priority = 0.8,
                    Url = urlbase + "Home/Noticias" + "/",
                    Frequency = SitemapFrequency.Hourly
                });
                data.noticias.Insert(0, data.noticia);
                foreach (Article article in data.noticias)
                {
                    string url = urlbase + "Home/Detallesnoticia?title=" + article.TitleUrl + "&fecha=" + article.FechaPubUrl + "/";
                    list.Add(new SitemapNode { LastModified = DateTime.Now, Priority = 0.8, Url = url, Frequency = SitemapFrequency.Hourly });
                }
              
                
                new SitemapDocument().CreateSitemapXML(list, _env.ContentRootPath);
                return "sitemap.xml file should be create in root directory";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return "error";
            }
        }

    }
}