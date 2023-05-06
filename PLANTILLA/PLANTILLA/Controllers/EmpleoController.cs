using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using PLANTILLA.Clases;
using PLANTILLA.Filters;
using PLANTILLA.Helpers;
using PLANTILLA.Models;
using PLANTILLA.Services;

namespace PLANTILLA
    .Controllers
{
    public class EmpleoController : Controller
    {
        readonly DataManager _datamanager;
        readonly ILogger<EmpleoController> _logger;
        readonly Service_DEDOMENA service;

        SwitchEmpleo swEmpleo;

        public EmpleoController(
            SwitchEmpleo swEmpleo,
            DataManager provm,
            ILogger<EmpleoController> logger,
            Service_DEDOMENA service
            )
        {
            this._datamanager = provm;
            this.swEmpleo = swEmpleo;
            this._logger = logger;
            this.service = service;

        }

        #region OFERTAS.USUARIOS
        public async Task<IActionResult> Ofertas()
        {
            ViewBag.MetaTitle = "Ofertas de empleo";
            ViewBag.MetaDescription = "Ofertas de empleo";
            ViewBag.MetaImg = "https://vigilanciayproteccion.website/assets/images/logo_card.png";

            try
            {
                Dictionary<string, string> provincias = _datamanager.GenerateProvinciasDict();
                ViewBag.Provincias = provincias;
                List<Empleo> ofertas = await service.CallApi<List<Empleo>>("Empleo/ofertas", null);
                return View(ofertas.Take(18));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return View(null);
            }
        }
        public async Task<IActionResult> OfertasProvincia(string provincia)
        {
            ViewBag.ProvinciaImg = provincia;
            ViewBag.ProvinciaName = _datamanager.GetProvinciaName(provincia);
            Dictionary<string, string> provincias = _datamanager.GenerateProvinciasDict();
            ViewBag.Provincias = provincias;
            List<Empleo> ofertas = await service.CallApi<List<Empleo>>("Empleo/ShowLast3Empleos", null);
            List<Empleo> empleo = await service.CallApi<List<Empleo>>(
                "Empleo/OfertasProvincia",
                null,
                new Dictionary<string, string>() {
                    {
                        "provincia" , provincia}
                    },
                null,
                "GET");

            ViewBag.TopOfertas = ofertas;

            var urli = "https://vigilanciayproteccion.website/assets/images/provincias/banderas/" + provincia + ".png";

            ViewBag.MetaTitle = "Ofertas de empleo en " + provincia;
            ViewBag.MetaDescription = "Ofertas de empleo en " + provincia;
            ViewBag.MetaImg = urli;

            return View(empleo);
        }

        //DETALLES DE OFERTA
        public async Task<IActionResult> DetallesOferta(string provincia, string titulo)
        {
            //ANTIGUA PAGINA DE DETALLESOFERTA
            //SE REDIRECCIONA A PAGINA OFERTA DIRECTAMENTE PARE EVITAR PERDIDA
            //DE ENLACES DE LA ANTIGUA VERSION
            return RedirectToAction("Oferta", "Empleo",
                new { provincia = provincia, fecha = titulo });
        }
        public async Task<IActionResult> Oferta(string provincia, string fecha, string? titulo)
        {
            try
            {
                Empleo empleo = await service.CallApi<Empleo>("empleo/searchempleo", null,
                    new Dictionary<string, string>() { { "fechastring", fecha }, { "provincia", provincia } }, null, "GET");
                if (empleo == null)
                {
                    ViewBag.MetaTitle = "Ofertas de empleo";
                    ViewBag.MetaDescription = "Ofertas de empleo";
                    ViewBag.MetaImg = "https://vigilanciayproteccion.website/assets/images/logo_card.png";
                    return View();
                }
                else
                {
                    List<Empleo> ofertas = await service.CallApi<List<Empleo>>(
                "Empleo/OfertasProvincia",
                null,
                new Dictionary<string, string>() {
                    {
                        "provincia" , provincia}
                    },
                null,
                "GET");
                    ViewBag.ProvinciaImg = provincia;
                    ViewBag.TopOfertas = ofertas;

                    var urli = "https://vigilanciayproteccion.website/assets/images/provincias/banderas/" + provincia + ".png";
                    ViewBag.MetaTitle = empleo.Titulo;
                    ViewBag.MetaDescription = empleo.Titulo;
                    ViewBag.MetaImg = urli;

                    empleo.ProvinciaName = _datamanager.GetProvinciaName(empleo.Provincia);
                    return View(empleo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);

                ViewBag.MetaTitle = "Ofertas de empleo";
                ViewBag.MetaDescription = "Ofertas de empleo";
                ViewBag.MetaImg = "https://vigilanciayproteccion.website/assets/images/logo_card.png";
                return View();
            }
        }
        #endregion
        #region OFERTAS.ADMINISTRACION

        [AuthorizeUsers]
        public async Task<IActionResult> OrganizeEmpleo()
        {
            await service.CallApi<bool>(
                "Empleo/Organize",
                HttpContext.Session.GetString("TOKEN_DEDOMENA"));
            return RedirectToAction("ManageEmpleo");
        }
        [AuthorizeUsers]
        public async Task<IActionResult> ManageEmpleo(string mensaje, bool? refresh, string? alert_message, string? alert_class)
        {
            //Objetos dela vista que no puedes ser nulos
            ViewBag.Categorias = _datamanager.GetCategorias();

            ViewBag.CountInfojobs = 0;
            ViewBag.CountOtros = 0;

            //VALIDACION CONTRA ICARO
            String token = HttpContext.Session.GetString("TOKEN_DEDOMENA");
            if (token == null)
            {
                try
                {
                    bool recoveredUser = await RecoverTokens();
                    if (!recoveredUser)
                    {
                        return RedirectToAction("Login", "Account", new { mensaje = "La sesión se cerró insesperadamente, pruebe a conectarse de nuevo" });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                    return RedirectToAction("Login", "Account", new { mensaje = "No se pudo iniciar sesión, el servicio no responde" });
                }
            }
            if (token != null)
            {
                ViewBag.Reload = refresh;
                if (refresh == true) //SE SOLICITA REFRESCAR LA TABLA BORRADORES
                {
                    //NO DEVOLVEMOS DATOS
                    //Se activará un JS que solicitará refrescar la tabla al Action RefreshRSSs()
                    //Mostrando un mensaje en el modal y deshabilitando cualquier click
                    ViewBag.Displayed = "block";
                    ViewBag.Mensaje = "Buscando ofertas...";
                    return View();
                }
                else //NO SE SOLICITA REFRESCAR LA TABLA BORRADORES
                {
                    //¿HAY MENSAJE PARA PONER EN EL MODAL?
                    if (mensaje != null)
                    {
                        ViewBag.Displayed = "block";
                        ViewBag.Mensaje = mensaje;
                    }
                    //¿HAY MENSAJE PARA PONER EN EL ALERT?
                    if (alert_message != null && alert_message != "")
                    {
                        ViewBag.Alert =
                        "<div class=\"alert alert-" + alert_class + " alert - dismissible fade show\" role=\"alert\">" +
                        "<strong >" + alert_message + "</strong>" +
                        "<button type = \"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" +
                        "<span aria-hidden=\"true\">&times;</span>" +
                        "</button>" +
                        "</div>";
                    }
                    //CARGA DATOS DE LAS TABLAS EMPLEO y EMPLEOBR
                    List<Empleo> empleo = await service.CallApi<List<Empleo>>("Empleo/ofertas", null);
                    List<EmpleoBR> empleoborrador = await service.CallApi<List<EmpleoBR>>(
                        "EmpleoBR/ofertas",
                        HttpContext.Session.GetString("TOKEN_DEDOMENA"));
                    ViewBag.Borradores = empleoborrador;

                    return View(empleo);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { mensaje = "La sesión se cerró insesperadamente, pruebe a conectarse de nuevo" });
            }

        }


        [AuthorizeUsers]
        public async Task<IActionResult> EditEmpleo(int id, string? Modo)
        {
            Dictionary<string, string> provincias = _datamanager.GenerateProvinciasDict();
            ViewBag.Provincias = provincias;

            ViewBag.Categorias = _datamanager.GetCategorias();
            switch (Modo)
            {
                case "BR":
                    EmpleoBR emp = await service.CallApi<EmpleoBR>(
                "EmpleoBR/SearchEmpleoId",
                HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                new Dictionary<string, string>
                    {
                        {"id", id.ToString() }
                    },
                    null,
                    "GET");
                    ViewBag.Modo = "GUARDAR BORRADOR";
                    Empleo e = swEmpleo.EmpleoBRToEmpleo(emp);
                    return View(e);
                case
                    "PU":
                    Empleo emp2 = await service.CallApi<Empleo>(
                "Empleo/SearchEmpleoId",
                HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                new Dictionary<string, string>
                    {
                        {"id", id.ToString() }
                    },
                    null,
                    "GET");
                    ViewBag.Modo = "GUARDAR Y PUBLICAR";

                    return View(emp2);
                default:
                    return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>No se puede editar esta oferta</h1>" });
            }
        }
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> EditEmpleo(Empleo emp, string Categoria, string? Modo)
        {
            try
            {
                if (Modo == "GUARDAR BORRADOR")
                {
                    EmpleoBR empbr = swEmpleo.EmpleoToEmpleoBR(emp);
                    bool modified = await service.CallApi<bool>(
                        "EmpleoBR/EditEmpleo",
                        HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                        null,
                        empbr,
                        "PUT");

                    if (modified) return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta modificada con exito</h1>" });
                    else return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Error al modificar la oferta</h1>" });

                }
                else if (Modo == "GUARDAR Y PUBLICAR")
                {
                    bool modified = await service.CallApi<bool>(
                   "Empleo/EditEmpleo",
                   HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                   null,
                   emp,
                   "PUT");
                    if (modified) return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta modificada con exito</h1>" });
                    else return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Error al modificar la oferta</h1>" });
                }
                else return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Error no controlado</h1>" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>No se puede editar esta oferta</h1>" });
            }
        }
        [AuthorizeUsers]
        public IActionResult InsertEmpleo()
        {
            ViewBag.Categorias = _datamanager.GetCategorias();
            Dictionary<string, string> provincias = _datamanager.GenerateProvinciasDict();
            ViewBag.Provincias = provincias;
            return View();
        }
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> InsertEmpleo(Empleo emp, String Categoria, String Modo)
        {

            switch (Modo)
            {
                case "PUBLICAR":
                    bool created = await service.CallApi<bool>(
                      "Empleo/InsertEmpleo",
                      HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                      null,
                      emp,
                      "POST");
                    if (created) return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta publicada con éxito</h1>" });
                    else return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta no publicada</h1>" });

                case "GUARDAR":
                    EmpleoBR empbr = swEmpleo.EmpleoToEmpleoBR(emp);
                    bool createdBR = await service.CallApi<bool>(
                        "EmpleoBR/InsertEmpleo",
                        HttpContext.Session.GetString("TOKEN_DEDOMENA"),
                        null,
                        empbr,
                        "POST");
                    if (createdBR) return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta publicada con éxito</h1>" });
                    else return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta no publicada</h1>" });

                default:
                    return RedirectToAction("ManageEmpleo", "Empleo", new { mensaje = "<h1>Oferta publicada con éxito</h1>" });
                    break;
            }

        }       
        #endregion
        #region OFERTAS.ADMINISTRACION.RSS'S
        [AuthorizeUsers]
        public async Task<int> RefreshRSSs()
        {
            try
            {
                string token_dedomena = HttpContext.Session.GetString("TOKEN_DEDOMENA");

                if (token_dedomena != null)
                {
                    int nuevas_ofertas = await service.CallApi<int>(
                        "Empleobr/RefreshRSSs",
                        token_dedomena);
                    return nuevas_ofertas;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion
        #region OFERTAS.ADMINISTRACION.JSCALLS
        [AuthorizeUsers]
        public async Task<bool> PostOferta(string url, string provincia, string titulo)
        {
            //LLamado desde JavaScript Empleo.ManageEmpleo.ICARO
            if (url == null || provincia == null || titulo == null)
            {
                return false;
            }
            provincia = _datamanager.GetProvinciaImg(provincia);
            String token_dedomena = HttpContext.Session.GetString("TOKEN_DEDOMENA");
            String token_icaro = HttpContext.Session.GetString("TOKEN_ICARO");
            if (token_dedomena != null)
            {
                bool activetoken = await service.CallApi<bool>(
                    "Auth/CheckAccess",
                    token_dedomena,
                    new Dictionary<string, string>
                    {
                        {"token_icaro",token_icaro }
                    },
                    null,
                    "GET");
                if (activetoken == true)
                {
                    bool publicado = await service.CallApi<bool>("empleo/PostOferta", token_dedomena,
                        new Dictionary<string, string>(){
                            {"token_icaro",token_icaro }
                        },

                        new OfertaModelPub()
                        {
                            provincia = provincia,
                            titulo = titulo,
                            url = url
                        }, "POST").ConfigureAwait(false);
                    return publicado;

                }
                else
                {
                    //token expirado en servidor
                    try
                    {
                        bool recoveredUser = await RecoverTokens();

                        if (recoveredUser)
                        {
                            bool publicado = await service.CallApi<bool>("empleo/PostOferta", token_dedomena,
                            new Dictionary<string, string>(){
                            {"token_icaro",token_icaro }
                            },
                            new OfertaModelPub()
                            {
                                provincia = provincia,
                                titulo = titulo,
                                url = url
                            }, "POST").ConfigureAwait(false);
                            return publicado;
                        }
                        else { return false; }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                        return false; //ERROR
                    }
                }
            }
            else
            {
                try
                {
                    bool activeuser = await RecoverTokens();
                    if (activeuser)
                    {
                        bool publicado = await service.CallApi<bool>("empleo/PostOferta", token_dedomena,
                           new Dictionary<string, string>(){
                                {"token",token_icaro }
                           },
                           new OfertaModelPub()
                           {
                               provincia = provincia,
                               titulo = titulo,
                               url = url
                           }, "POST").ConfigureAwait(false);
                        return publicado;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                    await HttpContext.SignOutAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme);
                    return false; //ICARO NO RESPONDE
                }
            }

        }
        [AuthorizeUsers]
        public async Task<Empleo> PublicarEnWeb(int id)
        {
            String token_dedomena = HttpContext.Session.GetString("TOKEN_DEDOMENA");
            String token_icaro = HttpContext.Session.GetString("TOKEN_ICARO");
            bool activetoken = await service.CallApi<bool>(
                   "Auth/CheckAccess",
                   token_dedomena,
                   new Dictionary<string, string>
                   {
                        {"token_icaro",token_icaro }
                   },
                   null,
                   "GET");

            if (activetoken)
            {

                try
                {
                    //RECIBIMOS EL EMPLEO INSERTADO EN db EMPLEO PARA HABILITAR CON JS 
                    //EL BOTON ENVIAR CON LOS NUEVOS DATOS
                    Empleo empleo_insertado = await service.CallApi<Empleo>(
                       "Empleobr/Publicar",
                       token_dedomena,
                       new Dictionary<string, string> { { "id", id.ToString() } },
                       null,
                       "POST");

                    var fecha = empleo_insertado.FechaString;
                    fecha = System.Net.WebUtility.UrlEncode(fecha);
                    var myurl = "https://vigilanciayproteccion.website/Empleo/Oferta?provincia=" + empleo_insertado.Provincia + "&fecha=" + fecha;

                    empleo_insertado.UrlVigi = myurl;
                    return empleo_insertado;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        [AuthorizeUsers]
        public async Task<bool> DeleteOferta(int id)
        {
            try
            {
                String token_dedomena = HttpContext.Session.GetString("TOKEN_DEDOMENA");
                String token_icaro = HttpContext.Session.GetString("TOKEN_ICARO");
                bool activetoken = await service.CallApi<bool>(
                       "Auth/CheckAccess",
                       token_dedomena,
                       new Dictionary<string, string>
                       {
                        {"token_icaro",token_icaro }
                       },
                       null,
                       "GET");

                if (activetoken)
                {
                    bool borrado = await service.CallApi<bool>(
                    "Empleo/deleteempleo",
                    token_dedomena,
                    new Dictionary<string, string> { { "id", id.ToString() } },
                    null,
                    "DELETE");
                    return borrado;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return false;
            }
        }
        [AuthorizeUsers]
        public async Task<bool> DeleteOfertaBR(int id)
        {
            try
            {
                String token_dedomena = HttpContext.Session.GetString("TOKEN_DEDOMENA");
                String token_icaro = HttpContext.Session.GetString("TOKEN_ICARO");
                bool activetoken = await service.CallApi<bool>(
                       "Auth/CheckAccess",
                       token_dedomena,
                       new Dictionary<string, string>
                       {
                        {"token_icaro",token_icaro }
                       },
                       null,
                       "GET");

                if (activetoken)
                {
                    bool borrado = await service.CallApi<bool>(
                    "Empleobr/deleteempleo",
                    token_dedomena,
                    new Dictionary<string, string> { { "id", id.ToString() } },
                    null,
                    "DELETE");
                    return borrado;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return false;
            }
        }
        #endregion
        #region OFERTTAS.ADMINISTRACION.FILTROOFERTAS
        [AuthorizeUsers]
        public async Task<IActionResult> Filtro()
        {
            return View();
        }
        [AuthorizeUsers]
        public async Task<List<string>> GetWords()
        {
            String token = HttpContext.Session.GetString("TOKEN_DEDOMENA");
            if (token == null)
            {
                try
                {
                    bool recoveredUser = await RecoverTokens();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);                  
                }
            }
            if (token != null)
            {
                List<string> words = await service.CallApi<List<string>>("Empleo/GetWordsFilter", token,
                       null, null, "GET").ConfigureAwait(false);
                return words;
            }
            else return null;
        }
        [AuthorizeUsers]
        public async Task<List<string>> InsertWord(string word)
        {
            String token = HttpContext.Session.GetString("TOKEN_DEDOMENA");
            if (token == null)
            {
                try
                {
                    bool recoveredUser = await RecoverTokens();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                }
            }
            if (token != null)
            {
                List<string> words = await service.CallApi<List<string>>("Empleo/InsertWordFilter", token, 
                    new Dictionary<string, string>
                    {
                        {"word", word }
                    }, 
                    null, 
                    "POST").ConfigureAwait(false);

                return words;
            }
            else return null;
        }
        [AuthorizeUsers]
        public async Task<List<string>> DeleteWord(string word)
        {
            String token = HttpContext.Session.GetString("TOKEN_DEDOMENA");
            if (token == null)
            {
                try
                {
                    bool recoveredUser = await RecoverTokens();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                }
            }
            if (token != null)
            {
                List<string> words = await service.CallApi<List<string>>("Empleo/DeleteWordFilter", token, 
                    new Dictionary<string, string>
                    {
                        {"word", word }
                    }, 
                    null, 
                    "DELETE").ConfigureAwait(false);

                return words;
            }
            else return null;
        }
        #endregion
        #region DEDOMENA.REFRESH_TOKENS
        public async Task<bool> RecoverTokens()
        {
            HttpContext.Session.Remove("TOKEN_ICARO");
            HttpContext.Session.Remove("TOKEN_DEDOMENA");


            string username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            string password = HttpContext.User.FindFirst(ClaimTypes.Hash).Value;

            string[] data = await service.GetTokens(
                    new LoginModel
                    {
                        UserName = username,
                        Password = password
                    }
                    );


            if (data != null)
            {
                string token_dedomena = data[0];
                HttpContext.Session.SetString("TOKEN_DEDOMENA", token_dedomena);
                HttpContext.Session.SetString("TOKEN_ICARO", data[1]);
                return true;
            }
            else return false;
        }
    }

    #endregion

}
