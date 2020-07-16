using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
             new List<IdentityResource>
             {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
             };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:44301/signin-oidc" },
                    //RedirectUris = { "https://localhost:44301/home/index" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:44301/signout-callback-oidc" },
                  

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         "api1"
                    }
                }
            };
    }
}
