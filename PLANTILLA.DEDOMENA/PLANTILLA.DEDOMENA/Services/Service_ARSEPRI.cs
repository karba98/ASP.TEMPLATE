using CodeHollow.FeedReader;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;
using PLANTIILLA.DEDOMENA.Repositories;

namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_ARSEPRI
    {
        //RepositoryEmpleo repo;
        DataManager datamng;

        public Service_ARSEPRI(RepositoryEmpresas repo, DataManager datamng)
        {
            this.datamng = datamng;
            //this.repo = repo;
        }
        

        public async Task<List<Empleo>> GetFeed(String? provincia)
        {
            //String format = "dddd, MMMM dd, yyyy";
            String format = "dd/MM/yyyy HH:mm:ss tt";
            try
            {
                var url = "";
                if (provincia != null)
                {
                    url = "https://arsepri.com/category/empleo/" + provincia + "/feed/";

                }
                else
                {
                    url = "https://arsepri.com/category/empleo/feed/";
                }
                var feed = await FeedReader.ReadAsync(url);
                List<Empleo> empleos = new List<Empleo>();
                foreach (var item in feed.Items)
                {
                    if (item.Link.Contains("noticias"))
                    {
                        continue;
                    }
                    String encoded = "";
                    encoded = item.Content;
                    var doc = new HtmlDocument();
                    string titulo = "";

                    doc.LoadHtml(encoded);
                    var mainnodes = doc.DocumentNode.ChildNodes;

                    var nodefirst = mainnodes.FirstOrDefault();
                    mainnodes.Remove(nodefirst);
                    try
                    {
                        titulo = item.Title;
                    }
                    catch (Exception exc)
                    {
                        string error = exc.StackTrace;
                        titulo = item.Title;
                    }
                    nodefirst = mainnodes[1];
                    mainnodes.Remove(nodefirst);
                    String desc = "";
                    foreach (var n in mainnodes)
                    {
                        desc += n.OuterHtml;
                    }
                    string provinciarss = "NF";
                    foreach(string pro in item.Categories)
                    {
                        try
                        {
                            provinciarss = pro;                       
                            if (provinciarss != null && provinciarss != "")
                            {
                                provincia = datamng.GetProvinciaImgNormalizer(provinciarss);

                                if(provincia!=null && provincia != "")
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            provinciarss = "NF";
                        }
                    }
                    if (provincia == null)
                    {
                        provincia = "NF";
                    }
                    Empleo e = new Empleo()
                    {

                        Descripcion = desc,
                        Titulo = titulo,
                        FechaPub = (DateTime)item.PublishingDate,
                        Url = SearchObjec(mainnodes, "href"),
                        Salario = 0,
                        Email = "",
                        Provincia = provincia,
                        Telefono = "",


                    };
                    e.FechaString = e.FechaPub.ToString(format);
                    //e.FechaString = e.FechaPub.ToString(format, new CultureInfo("es-ES"));
                    e.Categoria = setImg(e.Url);
                    if (titulo != "")
                    {
                        empleos.Add(e);
                    }
                }
                return empleos;
            }
            catch (Exception)
            {
                return new List<Empleo>();
            }
        }

        public String setImg(String encoded)
        {
            String image = "";
            string[] categorias = datamng.GetCategorias();
            foreach(string categoria in categorias)
            {
                if (encoded.Contains(categoria))
                {
                    image = "infojobs";
                    return image;
                }
            }
            image = "otros";
            return image;
        }
        static String SearchObjec(IEnumerable<HtmlNode> nodes, String attribute)
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
                            if (attr.Name == attribute)
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
    }
}
