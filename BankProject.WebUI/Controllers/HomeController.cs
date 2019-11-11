using BankProject.Data.Model;
using BankProject.Data.Repository;
using BankProject.Data.UnitOfWork;
using BankProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProject.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}