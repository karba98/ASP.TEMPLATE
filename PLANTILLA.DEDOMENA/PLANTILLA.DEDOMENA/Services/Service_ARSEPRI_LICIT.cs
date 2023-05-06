using CodeHollow.FeedReader;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_ARSEPRI_LICIT
    {
        const String format = "dddd,dd MMMM, yyyy";

        public async Task<List<Licitacion>> GetLicitaciones(String provincia)
        {

            try
            {
                var url = "https://arsepri.com/category/" + provincia + "-licitaciones/feed/";
                var feed = await FeedReader.ReadAsync(url);
                List<Licitacion> licitaciones = new List<Licitacion>();
                foreach (var item in feed.Items)
                {
                    try
                    {
                        String encoded = "";
                        encoded = item.Content;
                        var doc = new HtmlDocument();
                        doc.LoadHtml(encoded);

                        var nodes = doc.DocumentNode.SelectNodes("p");

                        nodes.Remove(nodes.Last());
                        String urlgob = checkNode(nodes);

                        string pattern = @"\b&amp;\b";
                        string replace = "&";
                        urlgob = Regex.Replace(urlgob, pattern, replace);


                        String text = "";
                        foreach (HtmlNode n in nodes)
                        {
                            if (!n.InnerHtml.Contains("e-mail"))
                            {
                                text += n.InnerHtml;
                            }
                        }
                        String date = ((DateTime)item.PublishingDate).ToString(format, new CultureInfo("es-ES"));
                        Licitacion licitacion = new Licitacion()
                        {
                            Descripcion = item.Description,
                            FechaPub = date,
                            Title = item.Title,
                            UrlGob = urlgob,
                            HtmlContent =
                            doc.DocumentNode.SelectNodes("h2").FirstOrDefault().OuterHtml +
                            "<hr/>" +
                            text
                        };
                        licitaciones.Add(licitacion);
                    }
                    catch(Exception ex){}
                }
                return licitaciones;
            }
            catch (Exception ex)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }

        static String checkNode(HtmlNodeCollection nodes)
        {
            String url = "";
            foreach (HtmlNode node in nodes)
            {
                foreach (HtmlNode n in node.ChildNodes)
                {
                    if (n.HasAttributes)
                    {
                        foreach (HtmlAttribute attr in n.Attributes)
                        {
                            if (attr.Name == "href")
                            {
                                url = attr.Value;
                                node.RemoveChild(n);
                                return url;
                            }
                        }
                    }


                }
            }
            return url;
        }

        internal async Task<Licitacion> GetLicitacion(string porvincia, string title)
        {
            try
            {

                List<Licitacion> licitaciones = await GetLicitaciones(porvincia);
                foreach (Licitacion l in licitaciones)
                {
                    if (l.Title.Equals(title))
                    {
                        return l;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                //throw new ArgumentException("Error: " + ex.StackTrace);
            }
        }
    }
}
