using System;
using System.Globalization;
using System.Web.Mvc;
using IdentityProvider.Models;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Web;

namespace IdentityProvider.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(WSFederationMessage message)
        {
            try
            {
                if (message != null)
                {
                    if (message.Action == WSFederationConstants.Actions.SignIn)
                    {
                        var requestMessage = (SignInRequestMessage) WSFederationMessage.CreateFromUri(Request.Url);

                        if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                        {
                            SecurityTokenService sts = new CustomSecurityTokenService(CustomSecurityTokenServiceConfiguration.Current);
                            SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, User, sts);
                            FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, System.Web.HttpContext.Current.Response);
                        }
                        else
                        {
                            throw new UnauthorizedAccessException();
                        }
                    }
                    else
                    {
                        if (message.Action == WSFederationConstants.Actions.SignOut)
                        {
                            var requestMessage = (SignOutRequestMessage) WSFederationMessage.CreateFromUri(Request.Url);
                            FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, User, requestMessage.Reply, System.Web.HttpContext.Current.Response);
                        }
                        else
                        {
                            throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                           "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.",
                                           String.IsNullOrEmpty(message.Action) ? "<EMPTY>" : message.Action,
                                           WSFederationConstants.Parameters.Action,
                                           WSFederationConstants.Actions.SignIn,
                                           WSFederationConstants.Actions.SignOut));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("An unexpected error occurred when processing the request. See inner exception for details.", exception);
            }

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }
    }
}