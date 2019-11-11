using BankProject.WebUI.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProject.WebUI.Attributes
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserSession.Info == null)
            {
                filterContext.Result = new RedirectResult("/login/index");
            }
        }
    }
}