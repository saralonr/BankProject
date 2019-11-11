using BankProject.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankProject.WebUI.Authorize
{
    public static class UserSession
    {
        public static Customer Info { get { return HttpContext.Current.Session["UserSession"] as Customer; } set { HttpContext.Current.Session["UserSession"] = value; } }
    }
}