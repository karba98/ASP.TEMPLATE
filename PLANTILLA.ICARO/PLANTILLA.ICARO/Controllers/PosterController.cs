using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using VYPPORTAL.DEDOMENA.Clases;
using PLANTILLA.ICARO.Clases;
using PLANTILLA.ICARO.Helpers;
using PLANTILLA.ICARO.Models;

namespace PLANTILLA.ICARO.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PosterController : ControllerBase
    {
        readonly FacebookPoster Facebookposter;
        readonly TelegramPoster Telegramposter;
        readonly TwitterPoster Twitterposter;
        readonly LinkedinPoster LinkedinPoster;
        readonly ILogger<PosterController> _logger;

        public PosterController(
            FacebookPoster Facebookposter,
            TelegramPoster Telegramposter,
             LinkedinPoster LinkedinPoster,
            TwitterPoster Twitterposter,
        ILogger<PosterController> logger)
        {
            this._logger = logger;
            this.Twitterposter = Twitterposter;
            this.Facebookposter = Facebookposter;
            this.Telegramposter = Telegramposter;
            this.LinkedinPoster = LinkedinPoster;
        }
        /// <summary>
        /// Recibe oferta y envía a redes sociales VYP
        /// </summary>
        /// <remarks>
        /// Recibe objeto OfertaModelPub
        /// </remarks>
        /// <param name="oferta">The oferta.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        public async Task<bool> PostOferta(OfertaModelPub oferta)
        {

            //if(oferta!=null)
            //{
            //    string title = oferta.titulo;
            //    title = title.Replace(" ", "-");
            //    title = Regex.Replace(title.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            //    oferta.url += "&titulo="+title;
            //    //Facebook
            //    string token = Facebookposter.GetAccessToken();
            //    bool facebookPosted = Facebookposter.PostMessage(token, oferta);
            //    if (facebookPosted)
            //    {
            //        _logger.LogDebug("Offer published Facebook OK");
            //    }
            //    else
            //    {
            //        _logger.LogDebug("Offer not published");
            //    }

            //    //Telegram
            //    bool published = await Telegramposter.SendMessage(oferta);
            //    if (published)_logger.LogDebug("Offer published Telegram OK");
            //    else _logger.LogDebug("Offer not published Telegram");

            //    //Twitter
            //    string output = await Twitterposter.Twitt("Nueva oferta de empleo: "+oferta.titulo + " \n" + oferta.url+ "\n" +
            //        "#.. #empleo #seguridad #Vigilante #empleoseguridad");
            //    if (output.Contains("error"))
            //    {
            //        _logger.LogDebug("Offer not published Twitter");
            //        _logger.LogDebug(output);

            //    }
            //    else _logger.LogDebug("Offer published Twitter OK");

            //    //Linkedin
            //    string post_org = await LinkedinPoster.CreatePost(oferta, "urn:li:organization:74678106");
            //    if(post_org != null && post_org.Contains("created"))
            //    {
            //        _logger.LogDebug("Offer published Linkedin COMP OK");

            //    }
            //    else
            //    {

            //        _logger.LogDebug("Offer not published Linkedin COMP");
            //        _logger.LogDebug(post_org);
            //    }
            //    string post_person = await LinkedinPoster.CreatePost(oferta, "urn:li:person:0tREKavQ9y");
            //    if (post_person != null && post_person.Contains("created"))
            //    {
            //        _logger.LogDebug("Offer published Linkedin PERS OK");

            //    }
            //    else
            //    {

            //        _logger.LogDebug("Offer not published Linkedin PERS");
            //        _logger.LogDebug(post_person);
            //    }
            //}

            
            return true;
        }
        /// <summary>
        /// Recibe IDs de Grupos donde se encuentra nuestra bot activo
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<string>> GetChatIds()
        {
            List<string> chats = await Telegramposter.GetChatIds();
            return chats;
        }
        /// <summary>
        /// Recibe Noticia de nuestro blog y envia a redes sociales
        /// </summary>
        /// <param name="noticia">The noticia.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("[action]")]
        //LA NOTICIA SE ENVIA DESDE word/word/wp-content/themes/zox-news
        //ver doc functions.php linea 1197
        public async Task<bool> PostNoticia(NoticiaModelPub noticia)
        {
            ////Noticia de VYP.ES a WEBSIT&E
            //noticia.url = noticia.url.Replace(".es/", ".website/home/detallesnoticia?fecha=");
            ////noticia.url = noticia.url.Replace("https://", "https://www.");
            //noticia.url = noticia.url.Remove(noticia.url.LastIndexOf('/'));
            //int lastSlash = noticia.url.LastIndexOf('/');
            //noticia.url = noticia.url.Remove (lastSlash,1);
            //noticia.url = noticia.url.Insert(lastSlash, "&title=/");
            //noticia.url = noticia.url + "/";


            //_logger.LogDebug(ToolKit.Serialize(noticia));
            //if (noticia != null && noticia.titulo!="" && noticia.url!="" 
            //    && noticia.titulo != null && noticia.url != null)
            //{
            //    _logger.LogDebug("Posting...");
            //    string token = Facebookposter.GetAccessToken();
            //    //SE HACE WRAPP DE NOTICIA COMO OFERTA PARA POSTEAR
            //    OfertaModelPub oferta = new OfertaModelPub()
            //    {
            //        provincia = null,
            //        titulo = noticia.titulo,
            //        url = noticia.url
            //    };
            //    //FaceBook
            //    Facebookposter.PostMessage(token, oferta);
            //    _logger.LogDebug("Post published Facebook OK");

            //    bool published = Telegramposter.SendMessageNoticia(noticia);

            //    _logger.LogDebug("Post published Telegram OK");

            //    //Twitter
            //    string output = await Twitterposter.Twitt(noticia.titulo+"\n\n" + noticia.url + "\n" +
            //        "#.. #noticias #seguridad #Vigilante");
            //    if (output.Contains("error"))
            //    {
            //        _logger.LogDebug("Post published Twitter NO");
            //        _logger.LogDebug(output);

            //    }
            //    else
            //    {
            //        _logger.LogDebug("Post published Twitter OK");

            //    }
            //    //Linkedin
            //    string post_org = await LinkedinPoster.CreatePost(oferta, "urn:li:organization:74678106");
            //    if (post_org != null && post_org.Contains("created"))
            //    {
            //        _logger.LogDebug("Post published Linkedin COMP OK");

            //    }
            //    else
            //    {

            //        _logger.LogDebug("Post not published Linkedin COMP");
            //        _logger.LogDebug(post_org);
            //    }
            //    string post_person = await LinkedinPoster.CreatePost(oferta, "urn:li:person:0tREKavQ9y");
            //    if (post_person != null && post_person.Contains("created"))
            //    {
            //        _logger.LogDebug("Post published Linkedin PERS OK");

            //    }
            //    else
            //    {

            //        _logger.LogDebug("Post not published Linkedin PERS");
            //        _logger.LogDebug(post_person);
            //    }
                return true;
            //}
            //else
            //{
            //    _logger.LogDebug("No existe la noticia");
            //    return false;
            //}
            
        }

        /// <summary>
        /// Prueba de respuesta del servicio
        /// </summary>
        /// <returns>true</returns>
        [Authorize]
        [HttpGet("[action]")]
        public async Task<bool> CheckAccess()
        {
            return true;
        }
        public string ClientIPAddr { get; private set; }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("[action]")]
        public string GetIP()
        {
            // Retrieve client IP address through HttpContext.Connection
            try
            {
                return HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
            
        }
        
    }

}
