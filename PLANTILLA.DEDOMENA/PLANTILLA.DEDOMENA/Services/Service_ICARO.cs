using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_ICARO
    {
        private String url;
        MediaTypeWithQualityHeaderValue header;
        private Uri Uriapi;
        private MediaTypeWithQualityHeaderValue Header;
        public Service_ICARO(String url)
        {
            Uriapi = new Uri(url);
            Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public async Task<String> GetToken(String username, String password)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = Uriapi;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    LoginModel login = new LoginModel()
                    {
                        UserName = username,
                        Password = password
                    };
                    String json = JsonConvert.SerializeObject(login);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    String request = "auth/login";
                    HttpResponseMessage response = await client.PostAsync(request, content);
                    if (response.IsSuccessStatusCode)
                    {
                        String data = await response.Content.ReadAsStringAsync();
                        JObject obj = JObject.Parse(data);
                        String token = obj.GetValue("response").ToString();

                        return token;

                    }
                    else return null;

                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> CallApi<T>(string request, string token, string urlpost, string provincia, string titulo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = Uriapi;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    OfertaModelPub nuevopost = new OfertaModelPub
                    {
                        url = urlpost,
                        provincia = provincia,
                        titulo = titulo
                    };
                    String json = JsonConvert.SerializeObject(nuevopost);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    await client.PostAsync(request, content);

                    return "Publicado";
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> CallApi<T>(string request, string token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    client.BaseAddress = Uriapi;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    HttpResponseMessage response = await client.GetAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        String data = await response.Content.ReadAsStringAsync();
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> Publicar(string token, string url, string titulo, string provincia)
        {
            String request = "poster/PostOferta";
            string publicando = await CallApi<string>(request, token, url, provincia, titulo);

            return publicando != null ? "Publicado" : "Error";
        }
        public async Task<bool> CheckAccess(string token)
        {
            String request = "poster/CheckAccess";
            string allowed = await CallApi<string>(request, token);
            if (allowed != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
