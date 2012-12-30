using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Web.Configuration;
using System.Web.Security;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace IdentityProvider.Models
{
    public class CustomSecurityTokenService : SecurityTokenService
    {
        private const bool EnableAppliesToValidation = true;
        static readonly string[] PassiveRedirectBasedClaimsAwareWebApps = { /*"https://localhost/PassiveRedirectBasedClaimsAwareWebApp"*/ };

        public CustomSecurityTokenService(SecurityTokenServiceConfiguration configuration) : base(configuration)
        {
        }

        void ValidateAppliesTo( EndpointAddress appliesTo )
        {
            if (appliesTo == null)
            {
                throw new ArgumentNullException("appliesTo");
            }

            if (EnableAppliesToValidation)
            {
                bool validAppliesTo = false;

                foreach (var rpUrl in PassiveRedirectBasedClaimsAwareWebApps)
                {
                    if (appliesTo.Uri.Equals(new Uri(rpUrl)))
                    {
                        validAppliesTo = true;
                        break;
                    }
                }

                if (!validAppliesTo)
                {
                    throw new InvalidRequestException( String.Format( "The 'appliesTo' address '{0}' is not valid.", appliesTo.Uri.OriginalString ) );
                }
            }
        }

        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            ValidateAppliesTo(request.AppliesTo);
            var scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials);
            var encryptingCertificateName = WebConfigurationManager.AppSettings["EncryptingCertificateName"];

            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName));
            }
            else
            {
                scope.TokenEncryptionRequired = false;            
            }

            scope.ReplyToAddress = scope.AppliesToAddress;
            return scope;
        }

        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            if (null == principal)
            {
                throw new ArgumentNullException("principal");
            }

            var outputIdentity = new ClaimsIdentity();
            var roles = new RolePrincipal(principal.Identity);

            outputIdentity.Claims.Add(new Claim(ClaimTypes.Name, principal.Identity.Name));
            outputIdentity.Claims.Add(new Claim(ClaimTypes.Role, roles.GetRoles().First()));

            return outputIdentity;
        }
    }
}
