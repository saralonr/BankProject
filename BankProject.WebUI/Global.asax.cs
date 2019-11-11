using BankProject.Data.Model;
using BankProject.Data.Services;
using BankProject.Service;
using BankProject.WebUI.Authorize;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BankProject.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }
        void Session_Start()
        {
            HttpCookie user = Request.Cookies["User"];
            if (user != null)
            {
                string TCKN = user.Values["TCKN"];
                Guid skey = Guid.Parse(user.Values["SKey"]);

                ICustomerService _customerService = new CustomerService();
                Customer _customer = _customerService.CookieControl(new Data.DTO.LoginDTO() { SecretKey = skey,TCKN=TCKN });

                if (_customer != null)
                {
                    UserSession.Info = _customer;
                }
                else
                {
                    user.Expires = DateTime.Now.AddDays(-30);
                    Response.Cookies.Add(user);
                    UserSession.Info = null;
                    Session.Abandon();
                }

            }

        }
    }
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel(new NinjectBindingModule());
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }
    }

    public class NinjectBindingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ICustomerService>().To<CustomerService>();
            Kernel.Bind<IAccountService>().To<AccountService>();
            Kernel.Bind<IExternalService>().To<ExternalService>();
        }
    }
}
