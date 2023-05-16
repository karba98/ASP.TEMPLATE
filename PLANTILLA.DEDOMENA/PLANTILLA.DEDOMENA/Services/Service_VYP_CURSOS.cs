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
    public class Service_VYP_CURSOS
    {

        public async Task<List<Curso>> GetCursos()
        {
            var format = "dddd, dd MMMM , yyyy";
            try
            {
                List<Curso> crs = await GetFeed("");
                return crs;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<List<Curso>> GetCursos(string categoria)
        {
            var format = "dddd, dd MMMM , yyyy";
            try
            {
                List<Curso> crs = await GetFeed(categoria);
                return crs;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Curso> GetLastCurso()
        {
            var format = "dddd, dd MMMM , yyyy";
            try
            {
                List<Curso> crs = await GetFeed("");
                return crs.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<Curso> GetCurso(string f, string title)
        {
            var format = "dddd, dd MMMM , yyyy";
            try
            {
                var url = "https://..//" + f + title;
                if (f == null && title == null) return null;
                url += "feed?withoutcomments=1";
                var feed = await FeedReader.ReadAsync(url);
                var item = feed.Items.FirstOrDefault();

                String encoded = "";
                encoded = item.Content;
                var doc = new HtmlDocument();
                doc.LoadHtml(encoded);
                var images = doc.DocumentNode.SelectNodes("img");
                images.FirstOrDefault().Attributes.Add("style", "width:60%;margin-left:auto;margin-right:auto;");
                string text = doc.DocumentNode.InnerHtml;

                String titulo = item.Title;
                String urlcurso = item.Link;
                DateTime fecha = (DateTime)item.PublishingDate;
                string fechapub = fecha.ToString(format, new CultureInfo("es-ES"));
                string img = images[0].Attributes[0].Value;

                //foreach (HtmlNode n in nodes)
                //{
                //    text += n.InnerText;
                //}

                Curso curso = new Curso()
                {
                    Titulo = titulo,
                    Body = text,
                    Url = urlcurso,
                    FechaPub = fechapub,
                    TitleUrl = urlcurso.Remove(0, 43),
                    FechaPubString = fecha.ToString("yyyy/MM/dd"),
                    Img = img
                };
                return curso;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
        public async Task<List<Curso>> GetFeed(string category)
        {
            var format = "dddd, dd MMMM , yyyy";
            try
            {
                List<Curso> cursos = new List<Curso>();
                var url = "https://..//category/cursos/" + category + "feed/";
                var feed = await FeedReader.ReadAsync(url);
                foreach (var item in feed.Items)
                {
                    String encoded = "";
                    encoded = item.Content;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(encoded);

                    var nodes = doc.DocumentNode.SelectNodes("p");
                    var images = doc.DocumentNode.SelectNodes("img");
                    String titulo = item.Title;
                    String text = "";
                    String urlcurso = item.Link;
                    DateTime fecha = (DateTime)item.PublishingDate;
                    string fechapub = fecha.ToString(format, new CultureInfo("es-ES"));
                    string img = images[0].Attributes[0].Value;

                    foreach (HtmlNode n in nodes)
                    {
                        text += n.InnerText;
                    }

                    Curso curso = new Curso()
                    {
                        Titulo = titulo,
                        Body = text,
                        Url = urlcurso,
                        TitleUrl = urlcurso.Remove(0, 43),
                        FechaPubString = fecha.ToString("yyyy/MM/dd"),
                        FechaPub = fechapub,
                        Img = img
                    };
                    cursos.Add(curso);
                }
                return cursos;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
    }

}
