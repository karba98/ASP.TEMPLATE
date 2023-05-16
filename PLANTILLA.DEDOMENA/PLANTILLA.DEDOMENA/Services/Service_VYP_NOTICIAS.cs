using CodeHollow.FeedReader;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;


namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_VYP_NOTICIAS
    {

        public async Task<Article> GetLastNoticia()
        {
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                List<Article> noticias = await GetArticles("https://..//category/noticias/noticias-de-seguridad/feed/");

                return noticias.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Article> GetLastNoticiaPolicia()
        {
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                List<Article> noticias = await GetArticles("https://..//category/noticias/otras-noticias/feed/");

                return noticias.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Article> GetNoticia(string fecha, string title)
        {
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                List<Article> noticias = await GetArticles("https://..//category/noticias/noticias-de-seguridad/feed/");
                List<Article> noticias2 = await GetArticles("https://..//category/noticias/otras-noticias/feed/");
                noticias.AddRange(noticias2);
                Article noticia = noticias.FirstOrDefault(x => x.FechaPubUrl == fecha && x.TitleUrl == title);
                return noticia;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Article> GetSentencia(string fecha, string title)
        {
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                List<Article> sentencias = await GetArticles("https://..//category/otros/sentencias/feed/");
                Article sentencia = sentencias.FirstOrDefault(x => x.FechaPubUrl == fecha && x.TitleUrl == title);
                return sentencia;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Article> GetArticuloCiber(string fecha, string title)
        {
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                List<Article> sentencias = await GetArticles("https://..//category/otros/ciberseguridad/feed/");
                Article sentencia = sentencias.FirstOrDefault(x => x.FechaPubUrl == fecha && x.TitleUrl == title);
                return sentencia;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Article> GetArticuloSLaboral(string fecha, string title)
        {
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                List<Article> sentencias = await GetArticles("https://..//category/otros/seguridad-laboral/feed/");
                Article sentencia = sentencias.FirstOrDefault(x => x.FechaPubUrl == fecha && x.TitleUrl == title);
                return sentencia;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<List<Article>> GetArticles(string url)
        {
            List<Article> noticias = new List<Article>();
            String format = "dddd,dd MMMM, yyyy";

            try
            {
                var feed = await FeedReader.ReadAsync(url);
                foreach (var item in feed.Items)
                {

                    String encoded = "";
                    encoded = item.Content;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(encoded);

                    var nodes = doc.DocumentNode.SelectNodes("p");
                    var images = doc.DocumentNode.SelectNodes("img");
                    string img = "https://jmsocialexpert.com/wp-content/uploads/2021/01/loogoo-Ultima-Hora.png";
                    if (images != null)
                    {
                        try
                        {
                            images.FirstOrDefault().Attributes.Add("style", "width:60%;margin-left:auto;margin-right:auto;");
                            img = images[0].Attributes[0].Value;
                            if (img == null != img.Equals(""))
                            {
                                img = "https://jmsocialexpert.com/wp-content/uploads/2021/01/loogoo-Ultima-Hora.png";
                            }
                        }catch(Exception ex)
                        {
                            img = "https://jmsocialexpert.com/wp-content/uploads/2021/01/loogoo-Ultima-Hora.png";
                        }

                    }
                    String titulo = item.Title;
                    String text = "";
                    string htmlbody = doc.DocumentNode.InnerHtml;
                    String urlnoticia = item.Link;
                    string fechapub = ((DateTime)item.PublishingDate).ToString(format, new CultureInfo("es-ES"));



                    foreach (HtmlNode n in nodes)
                    {
                        text += n.InnerText;
                    }

                    Article noticia = new Article
                    {
                        Titulo = titulo,
                        Body = text,
                        TitleUrl = urlnoticia.Remove(0, 43),
                        BodyHtml = htmlbody,
                        Url = urlnoticia,
                        FechaPub = fechapub,
                        FechaPubUrl = ((DateTime)item.PublishingDate).ToString("yyyy/MM/dd"),
                        Img = img
                    };
                    noticias.Add(noticia);
                }
                return noticias;

            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
      
    }
}
