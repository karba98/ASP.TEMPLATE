using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using PLANTILLA.Clases;
using PLANTILLA.Models;
using PLANTILLA.Services;

namespace PLANTILLA.Controllers
{
    public class RSSFeedController : Controller
    {
        /// <summary>
        /// Controller de RSS Feed de Website VYP
        /// </summary>
        readonly Service_DEDOMENA _service;
        readonly ILogger<RSSFeedController> _logger;
        public RSSFeedController( 
            ILogger<RSSFeedController> _logger,
            Service_DEDOMENA service)
        {
            this._logger = _logger;
            this._service = service;
        }
        /// <summary>
        /// RSS Ofertas de BBDD
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Rss()
        {
            try
            {
                string contentType = "application/xml";

                //List<Post> posts = (List<Post>)await _postsService.GetPosts();
                List<Post> posts  = await _service.CallApi<List<Post>>(
                        "VYP/GetOfertasAsPosts",
                        null);

                var content = await GetFeedDocument(posts);
                return Content(content, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return Content(null);
            }
        }
        /// <summary>
        /// RSSs Noticias.
        /// </summary>
        /// <returns>
        /// Genera RSS de noticias para campaña https://vigilanciayproteccion.es
        /// </returns>
        public async Task<IActionResult> RssNoticias()
        {
            try
            {
                string contentType = "application/xml";

                List<Post> posts = await _service.CallApi<List<Post>>(
                        "VYP/GetNoticiasAsPosts",
                        null);
                var content = await GetFeedDocument(posts);
                return Content(content, contentType);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return Content(null);
            }
        }
        /// <summary>
        /// Generador de XML que recibe una List de Post's y devuelve el XML como String.
        /// Convierte cada Post en un AtomEntry para formatear el XML
        /// </summary>
        /// <param name="posts">The posts.</param>
        /// <returns>String (XML)</returns>
        public async Task<string> GetFeedDocument(List<Post> posts)
        {
            try
            {
                //PREPARE THE FEED METADATA
                StringWriter sw = new StringWriter();
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    Async = true,
                    Indent = true,
                    Encoding = System.Text.Encoding.UTF8,
                    OmitXmlDeclaration = false,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    CloseOutput = false
                }))
                {
                    var rss = new RssFeedWriter(xmlWriter);

                    await rss.WriteTitle("VigilanciaYProteccion");
                    await rss.WriteDescription("Empleo");
                    await rss.WriteGenerator("Empleo");
                    await rss.WriteValue("link", "https://vigilanciayproteccion.website");

                    if (posts != null && posts.Count() > 0)
                    {
                        //ADD ITEMS TO THE FEED
                        var feedItems = new List<AtomEntry>();
                        foreach (var post in posts)
                        {
                            var item = ToRssItem(post, post.Url);
                            feedItems.Add(item);
                        }

                        foreach (var feedItem in feedItems)
                        {
                            await rss.Write(feedItem);
                        }
                    }
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }


                //EXTRACT THE XML DOCUMENT
                return sw.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return null;
            }
        }
        /// <summary>
        /// Convierte un Post en un AtomEntry (Item RSS) para formatear el XML
        /// </summary>
        /// <param name="post">Objeto Post.</param>
        /// <param name="host">Url del Post.</param>
        /// <returns></returns>
        private AtomEntry ToRssItem(Post post, string host)
        {
            try
            {
                var item = new AtomEntry
                {
                    Title = post.Title,
                    Description = post.Content,
                    Id = $"{host}",
                    Published = post.PublishedOn,
                    LastUpdated = post.PublishedOn,
                    ContentType = "html",
                };
                item.AddLink(new Microsoft.SyndicationFeed.SyndicationLink(new Uri($"{host}")));
                item.AddCategory(new Microsoft.SyndicationFeed.SyndicationCategory("Empleo"));
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                return null;
            }

        }
    }
}
