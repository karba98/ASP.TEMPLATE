
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using PLANTILLA.Models;

namespace PLANTILLA.Clases
{
    public class RSSReader
    {
        /// <summary>
        /// Lector de RSS's de noticias
        /// </summary>
        private readonly List<string> filter; // Lista de palabras a filtrar para RSS Noticias de varias fuentes
        public RSSReader()
        {
            //arrayd de palabras filtradas
            filter = new List<string>() {};

        }
    }
}
