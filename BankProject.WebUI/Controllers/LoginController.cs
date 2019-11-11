using BankProject.Data.DTO;
using BankProject.Data.Model;
using BankProject.Data.Services;
using BankProject.WebUI.Authorize;
using BankProject.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProject.WebUI.Controllers
{
    public class LoginController : Controller
    {
        #region Private Member
        private ICustomerService _customerService;
        #endregion
        #region CTOR
        public LoginController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion
        // GET: Login
        #region ACTION
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Logout()
        {
            if (UserSession.Info != null)
            {
                Session.Abandon();
                HttpCookie user = new HttpCookie("User");
                user.Expires = DateTime.Now.AddDays(-30);
                Response.Cookies.Add(user);
                UserSession.Info = null;
            }
            return Redirect("/");
        }
        #endregion

        #region POST
        [HttpPost]
        public ActionResult Index(LoginDTO dto)
        {
            if (UserSession.Info != null) return Redirect("/");

            if (string.IsNullOrWhiteSpace(dto.TCKN) || string.IsNullOrWhiteSpace(dto.Password))
            {
                TempData["Error"] = "Kullanıcı adı veya şifreniz yanlış.";
                return Redirect("/Login/Index");
            }
            else if (dto.TCKN.Length > 11 || dto.Password.Length > 50)
            {
                TempData["Error"] = "Kullanıcı adı veya şifreniz yanlış.";
                return Redirect("/Login/Index");
            }

            Customer _customer = _customerService.LoginControl(dto);

            if (_customer != null)
            {
                bool remember = Convert.ToBoolean(Request.Form["Remember"]);
                if (remember)
                {
                    HttpCookie user = new HttpCookie("User");
                    user.Expires = DateTime.Now.AddDays(30);
                    user.Values.Add("TCKN", _customer.TCKN);
                    user.Values.Add("SKey", _customer.SecretKey.ToString());
                    Response.Cookies.Add(user);
                }

                UserSession.Info = _customer;
                return Redirect("/");
            }
            else
            {
                TempData["Error"] = "Kullanıcı adı veya şifreniz yanlış.";
                return Redirect("/login/index");
            }
        }
        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            if (UserSession.Info != null) return Redirect("/");

            if (string.IsNullOrWhiteSpace(customer.TCKN) || string.IsNullOrWhiteSpace(customer.Password) || string.IsNullOrWhiteSpace(customer.Firstname) || string.IsNullOrWhiteSpace(customer.Lastname))
            {
                TempData["Error"] = "Lütfen bilgileri eksiksiz doldurunuz";
                return Redirect("/Login/Register");
            }
            if (customer.Password.Length<6)
            {
                TempData["Error"] = "Şifreniz 6 karakterden kısa olamaz.";
                return Redirect("/Login/Register");
            }
            if (!DataHelpers.TCKNCheck(customer.TCKN))
            {
                TempData["Error"] = "Girdiğiniz TCKN bilgisini kontrol ediniz.";
                return Redirect("/Login/Register");
            }
            bool result = _customerService.NewCustomer(customer);

            if (result)
            {
                TempData["Success"] = "Kayıt işlemi başarılı. Bilgilerinizle giriş yapabilirsiniz.";
                return Redirect("/Login/Index");
            }
            else
            {
                TempData["Error"] = "Bu TCKN ile daha önce kayıt olunmuş.";
                return Redirect("/Login/Register");
            }
        }
        #endregion

    }
}