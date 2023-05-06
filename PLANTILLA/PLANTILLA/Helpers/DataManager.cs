using System;
using System.Collections.Generic;
using System.Linq;

namespace PLANTILLA.Helpers
{
    public class DataManager
    {
        String[] FlagsName = new String[] { "A Coruña", "Lugo", "Ourense", "Pontevedra", "Asturias", "Cantabria", "Vizcaya", "Guipúzcoa",
                "Álava", "Navarra", "Huesca", "Zaragoza", "Teruel", "Barcelona", "Tarragona", "Lleida", "Girona", "León", "Zamora", "Salamanca",
                "Palencia", "Valladolid", "Ávila", "Burgos", "Soria", "Segovia", "La Rioja", "Madrid", "Cáceres", "Badajoz", "Guadalajara", "Cuenca",
                "Albacete", "Toledo", "Ciudad Real", "Castellon", "Valencia", "Alicante", "Murcia", "Huelva", "Sevilla", "Córdoba", "Jaén",
                "Cádiz", "Málaga", "Granada", "Almería", "Baleares", "Santa Cruz de Tenerife", "Las Palmas de Gran Canaria", "Ceuta", "Melilla" };
        String[] RoutesName = new String[] {"la_coruna", "Lugo", "Orense", "Pontevedra", "Asturias",
                "Cantabria", "vizcaya", "Guipuzcoa", "Alava", "Navarra", "Huesca", "Zaragoza", "Teruel",
                "Barcelona", "Tarragona", "Lerida", "Girona", "Leon", "Zamora", "Salamanca", "Palencia",
                "Valladolid", "avila", "Burgos", "Soria", "Segovia", "La_Rioja", "Madrid", "Caceres", "Badajoz",
                "Guadalajara", "Cuenca", "Albacete", "Toledo", "ciudad_real", "Castellon", "Valencia", "Alicante", "Murcia", "Huelva",
                "Sevilla", "Cordoba", "Jaen", "Cadiz", "malaga", "Granada", "Almeria", "islas_baleares", "tf", "las_palmas", "Ceuta", "Melilla"};

        String[] categorias = new String[]{
                "otros",
            "infojobs",
            "linkedin",
            "vigilanciayproteccion",
            "indeed",
            "vsofertasserias",
            "jobatus",
            "glassdoor",
            "jobandtalent",
            "tablondeanuncios",
            "infoempleo",
            "Jooble"
            };
        Dictionary<string, string> CategoriasCursos = new Dictionary<string, string>()
        {
            {"Vigilante de seguridad", "vigilante-de-seguridad" },
            { "Vigilante de explosivos", "vigilante-de-explosivos" },
            {"Escolta privado", "escolta-privado"},
            {"Detective privado", "detective-privado"},
            {"Director y Jefe de seguridad", "director-y-jefe-de-seguridad"}
        };
        public String[] GetCategorias()
        {
            return categorias;
        }
        public Dictionary<string,string> GetCategoriasCurso()
        {
            return CategoriasCursos;
        }
        public List<string> GetPorvicniasUrls()
        {
            return RoutesName.ToList();
        }
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            var dic = new Dictionary<TKey, TValue>();

            for (int i = 0; i < keys.Count(); i++)
            {
                dic.Add(keys.ElementAt(i), values.ElementAt(i));
            }

            return dic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
        public Dictionary<string, string> GenerateProvinciasDict()
        {

            Dictionary<string, string> provincias = Merge<string, string>(FlagsName, RoutesName);
            return provincias;
        }
        public string GetProvinciaName(string provincia)
        {
            Dictionary<string, string> porvincias = GenerateProvinciasDict();
            //return porvincias.FirstOrDefault(x => x.Value == provincia).Key;
            string p = porvincias.FirstOrDefault(x => String.Equals(x.Value, provincia, StringComparison.CurrentCultureIgnoreCase)).Key;
            if (p == null)
                p = porvincias.FirstOrDefault(x => String.Equals(x.Key, provincia, StringComparison.CurrentCultureIgnoreCase)).Key;
            return p;
        }
        public string GetProvinciaImgNormalizer(string provinciaName)
        {
            Dictionary<string, string> provincias = GenerateProvinciasDict();
            if (provinciaName.Contains("Balears") || provinciaName.Contains("Islas Baleares")){
                return provincias.FirstOrDefault(x => x.Key.Normalize().Trim().ToLower().Contains("baleares", StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            if (provinciaName.Contains("Castellón")){
                return provincias.FirstOrDefault(x => x.Key.Normalize().Trim().ToLower().Contains("castellon", StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            string provreceived = provinciaName.Normalize().Trim().ToLower();

            return provincias.FirstOrDefault(x => x.Key.Normalize().Trim().ToLower().Contains(provreceived, StringComparison.CurrentCultureIgnoreCase)).Value;
        }
        public string GetProvinciaImg(string provinciaName)
        {
            Dictionary<string, string> porvincias = GenerateProvinciasDict();
            string p = porvincias.FirstOrDefault(x => String.Equals(x.Key, provinciaName, StringComparison.CurrentCultureIgnoreCase)).Value;
            if (p == null)
               p = porvincias.FirstOrDefault(x => String.Equals(x.Value, provinciaName, StringComparison.CurrentCultureIgnoreCase)).Value;
            return p;
        }
        public string GetProvinciaRoute(string provinciaName)
        {
            Dictionary<string, string> porvincias = GenerateProvinciasDict();
            string route = porvincias.FirstOrDefault(x => String.Equals(x.Key, provinciaName, StringComparison.CurrentCultureIgnoreCase)).Value;
            if (route == null || route == "")
            {
                return provinciaName;
            }
            else
            {
                return route;
            }
        }


    }
}
