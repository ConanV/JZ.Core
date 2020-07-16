using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                new IdentityResources.Profile()
            };
        }

        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new []
            {
                new ApiResource("WebAPI", "J&Z WebAPI"),
                new ApiResource("clientservice", "CAS Client Service"),
                new ApiResource("productservice", "CAS Product Service"),
                new ApiResource("agentservice", "CAS Agent Service")
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("productservice")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },

                    Scopes = { "productservice" }
                },

              
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new[]
            {
                //new Client
                //{
                //    ClientId = "client1",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "WebAPI",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                //    IdentityServerConstants.StandardScopes.Profile},

                //},

                //// resource owner password grant client
                //new Client
                //{
                //    ClientId = "client2",
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "WebAPI",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                //    IdentityServerConstants.StandardScopes.Profile }
                //},
                //new Client
                //{
                //    ClientId = "client.api.service",
                //    ClientSecrets = new [] { new Secret("clientsecret".Sha256()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                //    AllowedScopes = new [] { "clientservice" ,IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                //    IdentityServerConstants.StandardScopes.Profile}
                //},
                new Client
                {
                    ClientId = "product.api.service",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                   
                      // where to redirect to after login
           
                    AllowedScopes = new [] {"clientservice", "productservice", IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                   IdentityServerConstants.StandardScopes.Profile  }
                }
                //,
                //new Client
                //{
                //    ClientId = "agent.api.service",
                //    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                //    AllowedScopes = new [] { "agentservice", "clientservice", "productservice",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                //    IdentityServerConstants.StandardScopes.Profile }
                //}
            };
        }


        public static IEnumerable<TestUser> GetUsers()
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };

            return new []
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "test1@hotmail.com",
                    Password = "test1password",
                    Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                },
               new TestUser
               {
                  SubjectId = "1",
                  Username = "jack",
                  Password = "123"
               },
               new TestUser
               {
                  SubjectId = "2",
                  Username = "bob",
                  Password = "password"
               }
            };
        }
    }
}
