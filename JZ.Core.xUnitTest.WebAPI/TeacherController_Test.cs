using JZ.Core.WebAPI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JZ.Core.xUnitTest.WebAPI
{
    public class TeacherController_Test
    {
        public HttpClient Client { get; }
        public ITestOutputHelper Output { get; }


        public TeacherController_Test(ITestOutputHelper outputHelper)
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());
            Client = server.CreateClient();
            Output = outputHelper;
        }

        [Fact]
        public async Task GetTeachersListById_D_ShouldBe_Ok()
        {
            // Arrange
            // var content = new StringContent(JsonConvert.SerializeObject(new { Name = "cxt", Age = 22 }), Encoding.UTF8, MediaTypeNames.Application.Json);
            // Act
            var response = await Client.GetAsync("/api/teacher/GetTeachersListById_D?id=1");

            // Output
            var responseTest = await response.Content.ReadAsStringAsync();

            Output.WriteLine(responseTest);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
