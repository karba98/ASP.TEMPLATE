using System.IO;
using System.Net;

namespace VYPPORTAL.DEDOMENA.Clases
{
    public static class TinyUrl
    {
        public static string CompressURL(string strURL)
        {
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create("http://tinyurl.com/api-create.php?url=" + strURL);
            string strResponse = null;

            try
            {
                HttpWebResponse objResponse = objRequest.GetResponse() as HttpWebResponse;
                StreamReader stmReader = new StreamReader(objResponse.GetResponseStream());

                strResponse = stmReader.ReadToEnd();
            }
            catch { }
            return strResponse;
        }
    }
}
