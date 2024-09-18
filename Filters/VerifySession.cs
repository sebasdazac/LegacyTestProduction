using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;

namespace SictegWarehouse.Filters
{
    public class VerifySession : ActionFilterAttribute
    {

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    var oUsuario = context.HttpContext.Session.GetString("Wh_user");
        //    try
        //    {
        //        if (oUsuario == null)
        //        {
        //            if (context.Controller is LoginController == false)
        //            {
        //                context.HttpContext.Response.Redirect("Login");
        //            }
        //        }
        //        base.OnActionExecuting(context);
        //    }
        //    catch (Exception)
        //    {
        //        context.Result = new RedirectResult("~/Login");
        //    }

        //}
    }
}
