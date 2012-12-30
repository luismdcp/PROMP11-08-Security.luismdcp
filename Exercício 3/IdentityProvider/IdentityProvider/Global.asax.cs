using System.Web.Mvc;
using System.Web.Routing;
using IdentityProvider.Models;
using Microsoft.IdentityModel.Protocols.WSFederation;

namespace IdentityProvider
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var binder = new WSFederationMessageBinder();
            ModelBinders.Binders[typeof(WSFederationMessage)] = binder;
            ModelBinders.Binders[typeof(AttributeRequestMessage)] = binder;
            ModelBinders.Binders[typeof(PseudonymRequestMessage)] = binder;
            ModelBinders.Binders[typeof(SignInRequestMessage)] = binder;
            ModelBinders.Binders[typeof(SignOutRequestMessage)] = binder;
            ModelBinders.Binders[typeof(SignOutCleanupRequestMessage)] = binder;

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}