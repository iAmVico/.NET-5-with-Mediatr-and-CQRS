using Microsoft.Graph.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Bootstrapper.Helpers
{
    public interface IAzureAuthProvider
    {
        ClientCredentialProvider GetProvider();
    }
}
