using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using PLANTILLA.ICARO.Models;
using PLANTILLA.ICARO.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using PLANTILLA.ICARO.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace PLANTILLA.ICARO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {

        RepositoryUsers repo;
        RepositoryTokens repo_codes;
        HelperToken helper;
        ILogger<AuthController> _logger;
        public AuthController(RepositoryUsers repo,HelperToken helper, ILogger<AuthController> _logger, RepositoryTokens repo_codes)
        {
            this._logger = _logger;
            this.repo = repo;
            this.helper = helper;
            this.repo_codes = repo_codes;
        }

        //[HttpPost]
        //[Route("[action]")]
        //public bool Register(LoginModel user)
        //{
        //    repo.Register(user.UserName, user.Password);
        //    return true;
        //}
        //        

        /// <summary>
        /// Acceso.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Access Token</returns>
        [HttpPost]
        [Route("[action]")]
        public ActionResult Login(LoginModel model)
        {
            _logger.LogDebug("user "+model.UserName+" joined");
            User usuario =
                this.repo.CheckUser(model.UserName
                , model.Password);
            if (usuario != null)
            {

                Claim[] claims = new[]
                {
                new Claim("UserData",
                JsonConvert.SerializeObject(usuario))
                };
                DateTime estancia = default(DateTime);
                if (usuario.UserName != "WordpressAdmin")
                {
                    estancia = DateTime.UtcNow.AddMinutes(15);
                }
                else
                {
                    estancia = DateTime.UtcNow.AddSeconds(30);
                    _logger.LogDebug("Worpress logged, the token has 30 secs remaning");
                }
                try
                {
                    JwtSecurityToken token = new JwtSecurityToken
                        (
                            issuer: helper.Issuer
                            , audience: helper.Audience
                            , claims: claims
                            , expires: estancia
                            , notBefore: DateTime.UtcNow
                            , signingCredentials:
                                new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(
                        new
                        {
                            response =
                            new JwtSecurityTokenHandler().WriteToken(token)
                        });
                }catch(Exception ex)
                {
                    _logger.LogError("Error generating token");
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// Registra un nuevo codigo para usar en RRSS
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="key">The key.</param>
        [Authorize]
        [HttpPost("[action]")]
        public void RegisterCode(string code,string key)
        {
            repo_codes.RegisterCode(key, code);
        }
        /// <summary>
        /// Obtiene el codigo de una de las RRSS.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]")]
        public string GetCode(string key)
        {
            /// FacebookApiId
            /// FacebookApiSecret
            /// FacebookLongLifeToken
            /// FaceBookUserID
            /// LinkedInClientId
            /// LinkedInClientSecret
            /// LinkedIntRefreashToken
            /// TelegramTokenBot
            /// TwitterAccessToken
            /// TwitterAccessTokenSecret
            /// TwitterApiSecret
            /// TwitterApiToken
            /// 
            return repo_codes.GetCode(key);
        }
        /// <summary>
        /// Actualiza el token/codigo de una de las RRSS
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="code">Nuevo código.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("[action]")]
        public string UpdateCode(string key,string code)
        {
            return repo_codes.UpdateCode(key, code);
        }
    }
}