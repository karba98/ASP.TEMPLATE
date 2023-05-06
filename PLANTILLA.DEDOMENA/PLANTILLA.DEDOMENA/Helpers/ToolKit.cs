using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public class ToolKit
    {
        public static bool CompareBytes(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length) iguales = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Equals(b[i]) == false) { iguales = false; break; }
            }
            return iguales;
        }
        public static string NormalizeFileName(string FileName)
        {
            return null;
        }
        public static string Serialize(object objeto)
        {
            return JsonConvert.SerializeObject(objeto);

        }
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);

        }
        
    }
}
