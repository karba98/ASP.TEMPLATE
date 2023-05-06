using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PLANTILLA.Helpers;
using PLANTILLA.Models;
using PLANTILLA.Services;

namespace PLANTILLA.Controllers
{
    public class AccountController : Controller
    {
        Service_DEDOMENA service_dedomena;

        readonly ILogger<AccountController> _logger;
        public AccountController(Service_DEDOMENA service_dedomena, ILogger<AccountController> _logger)
        {
            this.service_dedomena = service_dedomena;
            this._logger = _logger;
        }
        public IActionResult Login(string mensaje)
        {
            if (mensaje != null)
            {
                ViewBag.Mensaje = mensaje;
                ViewBag.Displayed = "block";
            }
            else
            {
                ViewBag.Displayed = "none";
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(String username, String password)
        {
            HttpContext.Session.Remove("TOKEN_DEDOMENA");
            HttpContext.Session.Remove("TOKEN_ICARO");
            String[] data = new string[2];
            try
            {
                data = await service_dedomena.GetTokens(
                    new LoginModel
                    {
                        UserName = username,
                        Password = password
                    }
                    );

                string token_dedomena = data[0];
                if (token_dedomena == "401")
                {
                    ViewBag.Mensaje = "Usuario / Password Incorrectos";
                    return View();
                }
               
                else
                {
                    HttpContext.Session.SetString("TOKEN_ICARO", data[1]);
                    ClaimsIdentity identidad =
                    new ClaimsIdentity(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  ClaimTypes.Name,
                  ClaimTypes.Role
                  );
                    identidad.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
                    identidad.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    identidad.AddClaim(new Claim(ClaimTypes.Name, username));
                    identidad.AddClaim(new Claim(ClaimTypes.Hash, password));

                    ClaimsPrincipal user = new ClaimsPrincipal(identidad);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        user,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.Now.AddMinutes(15)
                        }
                        );
                    HttpContext.Session.SetString("TOKEN_DEDOMENA", token_dedomena);
                    return RedirectToAction("ManageEmpleo", "Empleo");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message+"\n"+ex.StackTrace+"\n"+ex.InnerException);
                ViewBag.Mensaje = "El servicio de validación no responde, pruebe más tarde";
                return View();
            }

        }
        //public IActionResult Register()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Register(String UserName, String Password)
        //{
        //    repo.Register(UserName, Password);
        //    return View();
        //}
        public async Task<IActionResult> CerrarSesion()
        {
            HttpContext.Session.Remove("TOKEN_DEDOMENA");
            HttpContext.Session.Remove("TOKEN_ICARO");
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account", new { mensaje = "Sessión cerrada correctamente" });
        }
    }
}
