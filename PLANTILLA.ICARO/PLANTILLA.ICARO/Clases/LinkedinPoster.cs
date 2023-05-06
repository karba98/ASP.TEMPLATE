using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PLANTILLA.ICARO.Models;
using PLANTILLA.ICARO.Repositories;

namespace PLANTILLA.ICARO.Clases
{
    public class LinkedinPoster
    {
        private MediaTypeWithQualityHeaderValue Header;
        readonly String refreshToken;
        readonly String clienID;
        readonly String clientSecret;
        RepositoryTokens repo_codes;

        public LinkedinPoster(RepositoryTokens repo_codes)
        {
            this.repo_codes = repo_codes;
            this.refreshToken = repo_codes.GetCode("LinkedIntRefreashToken");
            this.clientSecret = repo_codes.GetCode("LinkedInClientSecret");
            this.clienID = repo_codes.GetCode("LinkedInClientId");

            Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetToken()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.linkedin.com/oauth/v2/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);

                    var parameters = new Dictionary<string, string>
                    {
                        {"grant_type", "refresh_token" },
                        {"refresh_token", refreshToken },
                        {"client_id", clienID },
                        {"client_secret" , clientSecret }
                    };
                    var encodedcontent = new FormUrlEncodedContent(parameters);
                    HttpResponseMessage response = await client.PostAsync("accessToken", encodedcontent);

                    dynamic item = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    return item["access_token"];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> CreatePost(OfertaModelPub oferta,string target)
        {
            try
            {
                string access_token = await GetToken();

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.linkedin.com/v2/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);

                    client.DefaultRequestHeaders.Add("Authorization", "Bearer "+access_token);

                    String json = JsonConvert.SerializeObject(new
                    {
                        content = new
                        {
                            contentEntities = new dynamic [] 
                            {
                                new
                                {
                                    entityLocation = oferta.url,
                                    thumbnails = new dynamic [] 
                                    {
                                        new
                                        {
                                            resolvedUrl = oferta.url
                                        }
                                    }
                                }
                            },
                            title = oferta.titulo,
                            landingPageUrl = oferta.url
                        },
                        distribution = new
                        {
                            linkedInDistributionTarget = new {}
                        },
                        owner = target,
                        subject = oferta.titulo,
                        text = new
                        {
                            text=oferta.titulo+ "\n\n#seguridadprivada #vigilantedeseguridad #noticiasseguridad #noticias #Vigilante #empleoseguridad"
                        }
                    });
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("shares", content);

                    //dynamic item = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
