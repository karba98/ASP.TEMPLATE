using CodeHollow.FeedReader;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_INDEED
    {
        private const string indeed_rss = "wwwroot/indeed.xml";
        //private const string indeed_rss = "https://es.indeed.com/rss?q=teleoperador&sort=date&l=Espa%C3%B1a";
        //private const string indeed_rss = "wwwroot/indeed.xml";
        DataManager _datamanager;

        public Service_INDEED(DataManager _datamanager)
        {
            this._datamanager = _datamanager;
        }
        

        //ISO-8859-15 => 28605
        public async Task<List<EmpleoBR>> GetFeed()
        {
            String format = "dd/MM/yyyy HH:mm:ss tt";
            try
            {
                var url = indeed_rss;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Stream stream = null;
                try
                {

                    //Descargamos el rss como fichero (actualizamos el existente)
                    WebClient client = new WebClient();
                    client.Headers.Add(HttpRequestHeader.Accept, "*/*");
                    client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                    client.Headers.Add("authority", "es.indeed.com");
                    client.Headers.Add("method", "GET");
                    client.Headers.Add("path", "https");
                    client.Headers.Add(HttpRequestHeader.UserAgent, "useragentprueba");

                    stream = client.OpenRead(url);

                }
                catch (WebException e)
                {
                    Console.WriteLine(e);
                }
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var docu = new XmlDocument();
                    docu.Load(reader);

                    XmlDeclaration dec = docu.FirstChild as XmlDeclaration;
                    if (dec != null)
                    {
                        dec.Encoding = "UTF-8";
                    }
                    docu.Save(@"rss.indeed.autoupdate.xml");
                }

                //leemos feed desde fichero codificado a utf8
                Feed feed = await FeedReader.ReadFromFileAsync(@"rss.indeed.autoupdate.xml");

                List<EmpleoBR> empleos = new List<EmpleoBR>();
                foreach (var item in feed.Items)
                {
                    String encoded = "";

                    encoded = item.Description;
                    var doc = new HtmlDocument();
                    string titulo = "";

                    doc.LoadHtml(encoded);
                    var mainnodes = doc.DocumentNode.ChildNodes;

                    string descri = doc.ParsedText;
                

                    var nodefirst = mainnodes.FirstOrDefault();

                    try
                    {
                        titulo = item.Title;
                    }
                    catch (Exception exc)
                    {
                        titulo = item.Title;
                    }

                    int charLocation = titulo.IndexOf(" provincia", StringComparison.Ordinal);
                    //provincia
                    string provincia = "";
                    int indesofcoma = titulo.LastIndexOf(',');
                    if (indesofcoma == -1)
                    {
                        indesofcoma = titulo.LastIndexOf('-');
                    }
                    if (charLocation > 0)
                    {
                        provincia = titulo.Substring(indesofcoma+2, (charLocation-indesofcoma)-2);
                    }

                    try
                    {

                        provincia = _datamanager.GetProvinciaImgNormalizer(provincia);

                        if (provincia == null)
                        {
                            provincia = "NF";
                        }
                    }
                    catch (Exception) { provincia = "NF"; }

                    EmpleoBR e = new EmpleoBR()
                    {

                        Descripcion = descri,
                        Titulo = titulo,
                        FechaPub = (DateTime)item.PublishingDate,
                        Url = item.Link,
                        Salario = 0,
                        Email = "",
                        Provincia = provincia,
                        Telefono = "",
                        Modo = "B",
                        Categoria = "indeed"
                    };
                    e.FechaString = e.FechaPub.ToString(format);
                    if (titulo != "")
                    {
                        empleos.Add(e);
                    }
                }
                return empleos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<EmpleoBR>();
            }
        }
    }
}
