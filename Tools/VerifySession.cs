using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LegacyTest.Controllers;
using System;

namespace DannteNet6.Tools
{
    public class VerifySession : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var oUsuario = context.HttpContext.Session.GetString("");
            try
            {
                if (oUsuario == null)
                {
                    if (context.Controller is LoginController == false)
                    {
                        context.HttpContext.Response.Redirect("Login");
                    }           

                }
                base.OnActionExecuting(context);
            }
            catch (Exception)
            {
                context.Result = new RedirectResult("~/Login");
            }

        }
    }
}
