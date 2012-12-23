using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.IdentityModel.Claims;

namespace MvcApplication.CustomAttributes
{
    [AttributeUsageAttribute(AttributeTargets.Method)]
    public class RequireClaimsAttribute : ActionFilterAttribute
    {
        public string[] Claims { get; set; }

        public RequireClaimsAttribute(params string[] claims)
        {
            Claims = claims;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var principal = HttpContext.Current.User as IClaimsPrincipal;
            var buffer = new StringBuilder();

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                foreach (var claim in Claims)
                {
                    if (!principal.Identities[0].Claims.Any(c => c.ClaimType == claim))
                    {
                        buffer.AppendLine(String.Format("Claim '{0}' not provided.", claim));
                    }
                }

                if (buffer.Length > 0)
                {
                    var redirectTargetDictionary = new RouteValueDictionary
                        {
                            {"action", "Error"},
                            {"controller", "Home"},
                            {"message", buffer.ToString()}
                        };

                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                }
            }
        }
    }
}