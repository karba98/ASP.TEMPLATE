using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PLANTIILLA.DEDOMENA.Helpers;
using PLANTIILLA.DEDOMENA.Models;

namespace PLANTIILLA.DEDOMENA.Services
{
    public class Service_JOOBLE
    {
        private string apiKey = "b01925e3-c6fc-42c3-8437-ebd694122cdf";
        private string apikey;
        private DataManager _provicniasManager;
        private ILogger<Service_JOOBLE> logger;

        public Service_JOOBLE(DataManager provicniasManager, ILogger<Service_JOOBLE> logger)
        {
            this.logger = logger;
            this._provicniasManager = provicniasManager;
        }

        public async Task<List<EmpleoBR>> GetSecurityGuardJobs()
        {
            try{
                var url = "https://es.jooble.org/api/";
                var key = apiKey;

                //create request object
                WebRequest request = HttpWebRequest.Create(url + key);
                //set http method
                request.Method = "POST";
                //set content type
                request.ContentType = "application/json";
                //create request writer
                var writer = new StreamWriter(request.GetRequestStream());
                //write request body
                writer.Write(@"{ keywords: 'teleoperador', location: 'Spain',exclude:'comercial',pagesize:50}");
                //close writer
                writer.Close();
                //get response reader
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());

                string json = "";
                //read response
                while (!reader.EndOfStream)
                {
                    json += reader.ReadLine();
                }   
                JobSearchResult results = ToolKit.Deserialize<JobSearchResult>(json);
                List<EmpleoBR> empleo = new List<EmpleoBR>();


                Dictionary<string,string> provincias = _provicniasManager.GenerateProvinciasDict();
                foreach(JoobleJob j in results.jobs){
                    string p = "NF";
                    try{
                        string[] words = j.location.Split(" ");
                        foreach(string word in words){
                            bool existProvinciaVals = provincias.Values.Any(val => val.Equals(word));
                            bool existProvinciaKeys = provincias.Keys.Any(val => val.Equals(word));

                            if(existProvinciaKeys){
                                string found = _provicniasManager.GetProvinciaRoute(word);
                                p=found;
                            }
                            else if(existProvinciaVals){
                                p=word;
                            }
                            if(!p.Equals("NF")){
                                break;
                            }
                        }
                        if(p.Equals("NF")){
                            string[] words2 = j.snippet.Split(" ");
                            foreach(string word in words2){
                                bool existProvinciaVals = provincias.Values.Any(val => val.Equals(word));
                                bool existProvinciaKeys = provincias.Keys.Any(val => val.Equals(word));

                                if(existProvinciaKeys){
                                    string found = _provicniasManager.GetProvinciaRoute(word);
                                    p=found;
                                }
                                else if(existProvinciaVals){
                                    p=word;
                                }
                                if(!p.Equals("NF")){
                                    break;
                                }
                            }
                        }
                    }
                    catch(Exception ex){
                        logger.LogError(ex.StackTrace);
                    }
                    EmpleoBR empl = new EmpleoBR(){
                        Categoria = "Jooble",
                        Descripcion = j.snippet,
                        Email = null,
                        FechaPub = j.updated,
                        Url = j.link,
                        Titulo = j.title+ " en "+j.location,
                        Provincia = p,
                        Telefono = null,
                        Salario = 0,
                    };
                    if (empl.Titulo.Contains("vigilante") ||
                        empl.Titulo.Contains("Vigilante") ||
                        empl.Titulo.Contains("vigilante de") ||
                        empl.Titulo.Contains("Vigilante de") ||
                        empl.Descripcion.Contains("vigilante") ||
                        empl.Descripcion.Contains("Vigilante") ||
                        empl.Descripcion.Contains("vigilante de") ||
                        empl.Descripcion.Contains("Vigilante de")
                        )
                        empleo.Add(empl);
                    
                }
                return empleo; 
            }
            catch(Exception ex){
                logger.LogError(ex.StackTrace);
                return null;
            }
        }
    }
    public class JobSearchResult
    {
        public int totalCount { get; set; }
        public JoobleJob[] jobs { get; set; }
    }

    public class JoobleJob
    {
        public string title { get; set; }
        public string location { get; set; }
        public string snippet { get; set; }
        public string salary { get; set; }
        public string source { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public string company { get; set; }
        public DateTime updated { get; set; }
        public long id { get; set; }
    }
}