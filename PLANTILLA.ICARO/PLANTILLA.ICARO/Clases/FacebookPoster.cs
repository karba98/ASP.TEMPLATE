
using Facebook;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using PLANTILLA.ICARO.Helpers;
using PLANTILLA.ICARO.Models;
using PLANTILLA.ICARO.Repositories;

namespace PLANTILLA.ICARO.Helpers
{
    public class FacebookPoster
    {
        //private const string AuthenticationUrlFormat = "https://graph.facebook.com/102092178830814/?fields=access_token&access_token={1}";
        //private const string ShortLifeToken =
        //   "https://graph.facebook.com/v12.0/oauth/access_token?grant_type=fb_exchange_token&client_id={0}&client_secret={1}";
        //private const string LongLifeToken =
        //   "https://graph.facebook.com/v12.0/oauth/access_token?grant_type=fb_exchange_token&client_id={0}&client_secret={1}&access_token={2}&redirect_uri=";
        //public string FacebookSecret;
        //public string FacebookApid;
        public string FaceBookUserID;
        public string FacebookLongLifeToken;
        readonly ILogger<FacebookPoster> _logger;


        RepositoryTokens repo_codes;

        public FacebookPoster(RepositoryTokens repo_codes, ILogger<FacebookPoster> logger)
        {
            this.repo_codes = repo_codes;
            this.FaceBookUserID = repo_codes.GetCode("FaceBookUserID");
            this.FacebookLongLifeToken = repo_codes.GetCode("FacebookLongLifeToken");

            this._logger = logger;
        }


        public string GetAccessToken()
        {
            try
            {
                string accessToken = string.Empty;
                string url = "https://graph.facebook.com/" + this.FaceBookUserID + "/?fields=access_token&access_token=" + FacebookLongLifeToken;

                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();

                    FacebookToken tok = ToolKit.Deserialize<FacebookToken>(responseString);
                    accessToken = tok.access_token;


                }
                if (accessToken.Trim().Length == 0)
                    throw new Exception("There is no Access Token");
                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.StackTrace + "\n" + ex.Message);
                return "";
            }

        }

        public bool PostMessage(string accessToken, OfertaModelPub oferta)
        //public string PostMessage(string accessToken, OfertaModelPub oferta)
        {
            //PageID 102092178830814
            try
            {
                Uri url = new Uri(oferta.url);
                string titulo = oferta.titulo;
                dynamic messagePost = new ExpandoObject();
                //messagePost.access_token = accessToken;
                //messagePost.picture = "https://vigilanciayproteccion.website/img/" + provincia + ".jpg";
                messagePost.link = url.AbsoluteUri;
                //messagePost.name = titulo;
                //messagePost.caption = titulo;
                messagePost.message = titulo;
                //messagePost.description = titulo;

                FacebookClient facebookClient = new FacebookClient(accessToken);

                try
                {
                    var result = facebookClient.Post("/4372119519521145/feed", messagePost);//GRUPO
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex.StackTrace + "\n" + ex.Message);
                }
                try
                {
                    var result2 = facebookClient.Post("/102092178830814/feed", messagePost);//PAGINA
                }catch(Exception ex)
                {
                    _logger.LogDebug(ex.StackTrace + "\n" + ex.Message);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.StackTrace + "\n" + ex.Message);
                return false;
                //return ex.Message + ex.StackTrace;
            }

        }


    }

}
