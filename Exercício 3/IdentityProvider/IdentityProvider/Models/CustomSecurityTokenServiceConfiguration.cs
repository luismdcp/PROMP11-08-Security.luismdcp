using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

namespace IdentityProvider.Models
{
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        static readonly object SyncRoot = new object();
        const string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";

        public static CustomSecurityTokenServiceConfiguration Current
        {
            get
            {
                var httpAppState = HttpContext.Current.Application;
                var customConfiguration = httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as CustomSecurityTokenServiceConfiguration;

                if (customConfiguration == null)
                {
                    lock (SyncRoot)
                    {
                        customConfiguration = httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as CustomSecurityTokenServiceConfiguration;

                        if (customConfiguration == null)
                        {
                            customConfiguration = new CustomSecurityTokenServiceConfiguration();
                            httpAppState.Add(CustomSecurityTokenServiceConfigurationKey, customConfiguration);
                        }
                    }
                }
            
                return customConfiguration;
            }
        }

        public CustomSecurityTokenServiceConfiguration() : base(WebConfigurationManager.AppSettings[Common.IssuerName], new X509SigningCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, WebConfigurationManager.AppSettings[Common.SigningCertificateName])))
        {
            SecurityTokenService = typeof(CustomSecurityTokenService);
        }
    }
}