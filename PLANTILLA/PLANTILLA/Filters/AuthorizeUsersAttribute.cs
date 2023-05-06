using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;

namespace PLANTILLA.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                //RECUPERAMOS EL ULTIMO SITIO DONDE HA PULSADO EL USUARIO
                ITempDataProvider provider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
                var TempData = provider.LoadTempData(context.HttpContext);

                String action = context.RouteData.Values["action"].ToString();
                String controller = context.RouteData.Values["controller"].ToString();

                TempData["action"] = action;
                TempData["controller"] = controller;

                provider.SaveTempData(context.HttpContext, TempData);

                context.Result = GetRoute("Login", "Account");

            }
            //else
            //{
            //    String action = context.RouteData.Values["action"].ToString();
            //    String controller = context.RouteData.Values["controller"].ToString();

            //}
        }
        public RedirectToRouteResult GetRoute(String action, String controller)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(new
            {
                action = action,
                controller = controller
            });
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}