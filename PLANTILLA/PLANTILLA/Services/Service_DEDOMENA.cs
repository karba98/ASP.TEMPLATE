using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using PLANTILLA.Models;
using PLANTILLA.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace PLANTILLA.Services
{
    public class Service_DEDOMENA
    {
        private String url;
        private Uri Uriapi;
        private MediaTypeWithQualityHeaderValue Header;

        private IHttpClientFactory HttpClientFactory { get; }
        private readonly IConfiguration configuration;
        public Service_DEDOMENA(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            configuration = config;
            url = configuration.GetConnectionString("PLANTILLA.DEDOMENA");
            HttpClientFactory = httpClientFactory;
            Uriapi = new Uri(url);
            Header = new MediaTypeWithQualityHeaderValue("*/*");
        }
       
        public async Task<T> CallApi<T>(string request, string token)
        {
            try
            {
                using (HttpClient client = HttpClientFactory.CreateClient())
                {

                    client.BaseAddress = Uriapi;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);

                    if(token!=null)
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    HttpResponseMessage response = await client.GetAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        String data = await response.Content.ReadAsStringAsync();
                        return ToolKit.Deserialize<T>(data);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        

        public async Task<T> CallApi<T>(string request, string token, Dictionary<string,string> paramlist, Object body,string type)
        {
            try
            {
                using (HttpClient client = HttpClientFactory.CreateClient())
                {

                    client.BaseAddress = Uriapi;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);
                    //token
                    if (token != null)
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    //En vez de pasar el token de ICARO como parametro lo pasamos como header
                    if (paramlist!=null && paramlist.Count > 0)
                    {
                        string token_icaro = null;
                        if (paramlist.ContainsKey("token_icaro"))
                        {
                            token_icaro = paramlist["token_icaro"];
                            //si estamos pasando el token ICARO como parametro, lo movemos a los
                            //headers y lo borramos de parametros
                            client.DefaultRequestHeaders.Add("token_icaro", token_icaro);
                            paramlist = null;
                        }
                    }
                    //parametros url
                    if (paramlist !=null)
                    {
                        request = QueryHelpers.AddQueryString(request, paramlist);
                    }
                    //body
                    string json = ToolKit.Serialize(body);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;
                    switch (type)
                    {
                        case "POST": response = await client.PostAsync(request, content); break;
                        case "PUT": response = await client.PutAsync(request, content); break;
                        case "DELETE": response = await client.DeleteAsync(request); break;
                        case "GET": response = await client.GetAsync(request); break;
                        default: response = await client.PostAsync(request, content); break;

                    }
                    if (response.IsSuccessStatusCode)
                    {
                        String data = await response.Content.ReadAsStringAsync();
                        return ToolKit.Deserialize<T>(data);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public async Task<string[]> GetTokens(LoginModel model)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    client.BaseAddress = Uriapi;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(Header);

                    //body
                    string json = ToolKit.Serialize(model);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response;
                    response = await client.PostAsync("auth/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        String data = await response.Content.ReadAsStringAsync();

                        Tokens tokens = ToolKit.Deserialize<Tokens>(data);

                        return new string[] {tokens.TOKEN_DEDOMENA,tokens.TOKEN_ICARO};
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
    class Tokens
    {
        public string TOKEN_DEDOMENA { get; set; }
        public string TOKEN_ICARO { get; set; }
    }
}


