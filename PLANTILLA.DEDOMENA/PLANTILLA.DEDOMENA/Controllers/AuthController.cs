using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading.Tasks;
using PLANTIILLA.DEDOMENA.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using PLANTIILLA.DEDOMENA.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PLANTIILLA.DEDOMENA.Services;
using PLANTIILLA.DEDOMENA.Repositories;

namespace PLANTIILLA.DEDOMENA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {

        RepositoryUsers repo;
        Service_ICARO service_icaro;
        HelperToken helper;

        public AuthController(Service_ICARO service_icaro,
            RepositoryUsers repo, 
            HelperToken helper)
        {
            this.helper = helper;
            this.service_icaro = service_icaro;
            this.repo = repo;
        }     
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {

            User usuario =
            this.repo.CheckUser(model.UserName
            , model.Password);
            if (usuario != null)
            {
                try
                {
                    string token_icaro = await service_icaro.GetToken(model.UserName, model.Password);

                    if (token_icaro != null)
                    {
                        Claim[] claims = new[]
                        {
                        new Claim("UserData",
                        JsonConvert.SerializeObject(usuario)),

                        };
                        DateTime estancia = default(DateTime);
                        if (usuario.UserName != "WordpressAdmin")
                        {
                            estancia = DateTime.UtcNow.AddMinutes(15);
                        }
                        else
                        {
                            estancia = DateTime.UtcNow.AddSeconds(30);
                        }
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
                                TOKEN_DEDOMENA =
                                new JwtSecurityTokenHandler().WriteToken(token),
                                TOKEN_ICARO =
                                token_icaro
                            }) ;

                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                catch (Exception ex)
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Ok(
                    new
                    {
                        TOKEN_DEDOMENA = "401",
                        TOKEN_ICARO = ""
                    });
            }


        }
        [Authorize]
        [HttpPost("[action]")]
        public bool Register(String UserName, String Password)
        {
            try
            {
                repo.Register(UserName, Password);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<int> CerrarSesion()
        {
            HttpContext.Session.Remove("TOKEN_ICARO");
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCodes.Status200OK;
        }
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<bool> CheckAccess()
        {
            string token_icaro = Request.Headers
                .Where(x => x.Key.Equals("token_icaro")).FirstOrDefault().Value;

            return await service_icaro.CheckAccess(token_icaro);
        }
    }
}