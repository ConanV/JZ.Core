using IdentityModel.Client;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JZ.Core.xUnitTest.WebAPI
{
    public class IdentityServer_Test
    {
        //public HttpClient IdentityClient { get; }
        //public HttpClient APIClient { get; }
        public ITestOutputHelper Output { get; }

        public IdentityServer_Test(ITestOutputHelper outputHelper)
        {
            //var server = new TestServer(WebHost.CreateDefaultBuilder()
            //    .UseStartup<JZ.Core.WebAPI.Startup>());
            //APIClient = server.CreateClient();

            //var server2 = new TestServer(WebHost.CreateDefaultBuilder()
            //   .UseStartup<JZ.IdentityServer.Startup>());
            //IdentityClient = server2.CreateClient();

            Output = outputHelper;
        }


        [Fact]
        public async Task GetToken_ShouldBe_Ok()
        {
            var IdentityClient = new HttpClient();

            var disco = await IdentityClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Output.WriteLine(disco.Error);
                Assert.True(disco.IsError);
                return;
            }

            // request token
            var tokenResponse = await IdentityClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Output.WriteLine(tokenResponse.Error);
                Assert.True(tokenResponse.IsError);
                return;
            }

            Output.WriteLine(tokenResponse.Json.ToString());
            Output.WriteLine("\n\n");

            // call api
            var APIClient = new HttpClient();
            APIClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await APIClient.GetAsync("https://localhost:44362/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Output.WriteLine(response.StatusCode.ToString());
                //Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();

                Output.WriteLine(content);
            }

            Assert.True(response.IsSuccessStatusCode);



        }

        //[Fact]
        //public async Task GetToken_ShouldBe_Ok()
        //{
        //    // Arrange
        //    var content = new StringContent(JsonConvert.SerializeObject(new {
        //        grant_type = "password",
        //        username = "jack",
        //        password="123",
        //        client_id= "client2",
        //        client_secret= "secret"
        //    }),
        //    Encoding.UTF8, MediaTypeNames.Application.Json);
        //    // Act
        //    var response = await Client.PostAsync("/connect/token",content);

        //    // Output
        //    var responseTest = await response.Content.ReadAsStringAsync();

        //    Output.WriteLine(responseTest);
        //    // Assert
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}
