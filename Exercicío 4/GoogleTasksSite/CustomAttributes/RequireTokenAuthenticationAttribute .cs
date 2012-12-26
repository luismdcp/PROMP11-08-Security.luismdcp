using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web;

namespace GoogleTasksSite.CustomAttributes
{
    public class RequireTokenAuthenticationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated && httpContext.User.Identity.AuthenticationType.Equals(AuthenticationTypes.Federation, StringComparison.OrdinalIgnoreCase);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            var message = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest(Guid.NewGuid().ToString(), filterContext.HttpContext.Request.RawUrl, false);
            filterContext.Result = new RedirectResult(message.RequestUrl);
        }
    }
}