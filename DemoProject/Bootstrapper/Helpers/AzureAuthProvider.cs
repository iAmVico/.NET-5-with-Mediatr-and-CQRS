using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Bootstrapper.Helpers
{
    public class AzureAuthProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IConfidentialClientApplication confidentialClientApplication;
        private readonly ClientCredentialProvider authProvider;

        public AzureAuthProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_configuration["AzureAd:ClientId"])
                .WithTenantId(_configuration["AzureAd:TenantId"])
                .WithClientSecret(_configuration["AzureAd:ClientSecretId"])
                .Build();

            authProvider = new ClientCredentialProvider(confidentialClientApplication);
        }

        public ClientCredentialProvider GetProvider()
        {
            return authProvider;
        }
    }
}
