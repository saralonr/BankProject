using BankProject.Data.Model;
using BankProject.Data.Services;
using BankProject.WebUI.Attributes;
using BankProject.WebUI.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProject.WebUI.Controllers
{
    [Auth]
    public class CustomerController : Controller
    {
        // GET: Customer
        #region Private Member
        #endregion
        #region CTOR
        public CustomerController()
        {

        }
        #endregion
        #region ACTION
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region POST

        #endregion
    }
}