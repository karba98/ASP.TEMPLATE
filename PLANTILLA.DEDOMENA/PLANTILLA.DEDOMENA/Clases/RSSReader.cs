
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Clases
{
    public class RSSReader
    {
        /// <summary>
        /// Lector de RSS's de noticias
        /// </summary>
        private readonly List<string> filter; // Lista de palabras a filtrar para RSS Noticias de varias fuentes
        public RSSReader()
        {
            filter = new List<string>() {
                "policia","policía","vigilante","seguridad privada","agente",
                " guardia ","detenido","detenida","detención","detencion",
                "arrest","ilegal",
                "droga","mossos","armas","alcohol"," robo "," roba ","machete",
                "violencia","atropello","atropellad","juzgado","juez"
            };

        }
        /// <summary>
        /// Extrae los datos de un RSS de noticias
        /// </summary>
        /// <param name="url">URL del feed de noticias.</param>
        /// <returns>List de Post's</returns>
        public List<Post> ParseRSSdotnet(string url)
        {
            SyndicationFeed feed = null;
            List<Post> posts = new List<Post>();
            try
            {
                using (var reader = XmlReader.Create(url))
                {
                    feed = SyndicationFeed.Load(reader);
                }
            }
            catch { return null; }

            if (feed != null)
            {
                foreach (var element in feed.Items)
                {
                    StringBuilder sb = new StringBuilder();
                    bool encoded = false;
                    foreach (SyndicationElementExtension extension in element.ElementExtensions)
                    {
                        XElement ele = extension.GetObject<XElement>();
                        if (ele.Name.LocalName == "encoded" && ele.Name.Namespace.ToString().Contains("content"))
                        {
                            sb.Append(ele.Value + "<br/>");
                            encoded = true;
                        }
                    }
                    string cont;
                    if (encoded) cont = sb.ToString();
                    else cont = element.Summary.Text;

                    Post post = new Post(
                        element.Title.Text,
                        cont,
                        cont,
                        null,
                        "fjcastro",
                        element.Links[0].Uri.ToString(),
                        element.PublishDate.DateTime);
                    posts.Add(post);
                }
                return posts;
            }
            else { return null; }
        }
        /// <summary>
        /// Filtra la lista de Post's devolviendo las que contienen
        /// las palabras clave de la List 'filter'.
        /// </summary>
        /// <param name="posts">Post's de entrada.</param>
        /// <returns>Post's filtrados por palabras clave</returns>
        public List<Post> Filter(List<Post> posts)
        {
            try
            {
                var postsfiltered = from p in posts
                                    where filter.Any(p.Content.ToLower().Contains)
                                    select p;
                postsfiltered = postsfiltered.OrderByDescending(p => p.PublishedOn);
                return postsfiltered.ToList();
            }
            catch (Exception)
            {
                return posts;
            }
        }
    }
}
