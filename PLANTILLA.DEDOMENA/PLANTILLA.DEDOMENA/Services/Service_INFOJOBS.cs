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
    public class Service_INFOJOBS
    {
        private const string infojobs_rss = "https://www.infojobs.net/trabajos.rss/kw_teleoperador?&sortBy=PUBLICATION_DATE&sinceDate=_24_HOURS";
        DataManager _datamanager;

        public Service_INFOJOBS(DataManager _datamanager)
        {
            this._datamanager = _datamanager;
        }
        
        //ISO-8859-15 => 28605
        public async Task<List<EmpleoBR>> GetFeed()
        {
            String format = "dd/MM/yyyy HH:mm:ss tt";
            try
            {
                var url = infojobs_rss;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                //Descargamos el rss como fichero (actualizamos el existente)
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(infojobs_rss);

                using (var reader = new StreamReader(stream, Encoding.GetEncoding(28605)))
                {
                    var docu = new XmlDocument();
                    docu.Load(reader);

                    XmlDeclaration dec = docu.FirstChild as XmlDeclaration;
                    if (dec != null)
                    {
                        dec.Encoding = "UTF-8";
                    }
                    docu.Save(@"rss.infojobs.autoupdate.xml");
                }

                //leemos feed desde fichero codificado a utf8
                Feed feed = await FeedReader.ReadFromFileAsync(@"rss.infojobs.autoupdate.xml");

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
                    int charLocation = descri.IndexOf('<', StringComparison.Ordinal);
                    //provincia
                    string provincia = "";
                    if (charLocation > 0)
                    {
                        provincia = descri.Substring(0, charLocation);
                    }

                    int parentesislocation = provincia.IndexOf('(');
                    int barlocation = 0;
                    try
                    {
                        barlocation = provincia.LastIndexOf('/');
                        if (barlocation == -1 || barlocation < parentesislocation)
                        {
                            barlocation = provincia.IndexOf(')');
                        }

                        int lengthToBar = provincia.Substring(0, barlocation).Length;
                        provincia = provincia.Substring(parentesislocation + 1, lengthToBar - parentesislocation - 1);

                        provincia = _datamanager.GetProvinciaImgNormalizer(provincia);
                        if (provincia == null)
                        {
                            provincia = "NF";
                        }
                    }
                    catch (Exception) { provincia = "NF"; }



                    var nodefirst = mainnodes.FirstOrDefault();

                    try
                    {
                        titulo = item.Title;
                    }
                    catch (Exception exc)
                    {
                        titulo = item.Title;
                    }

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
                        Categoria = "infojobs"
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
