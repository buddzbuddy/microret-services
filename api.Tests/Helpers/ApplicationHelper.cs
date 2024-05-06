using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace api.Tests.Helpers
{
    public static class ApplicationHelper
    {
        public static WebApplicationFactory<Program> CreateApplication()
            => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
            });
        public static WebApplicationFactory<Program> CreateApplicationMock(List<(Type _interface, dynamic implementer)> mocks)
        {
            return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureTestServices(services =>
                {
                    foreach ((Type i, dynamic impl) in mocks)
                    {
                        if (!services.Remove(services.First(d => d.ServiceType == i)))
                            throw new InvalidOperationException($"Cannot remove service {i.Name} prior to mock");
                        services.AddScoped(i, (servProv) => impl);
                    }
                });
            });
        }

        public static HttpClient CreateHttpClientJson(
            this WebApplicationFactory<Program> application) =>
            application.createHttpClientJson();
        static HttpClient createHttpClientJson(this WebApplicationFactory<Program> application)
        {
            var client = application.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptLanguage
                .Add(new StringWithQualityHeaderValue("en"));
            return client;
        }
        public static HttpClient CreateHttpClientJson(
            this WebApplicationFactory<Program> application, string bearerToken)
        {
            var client = application.createHttpClientJson();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", bearerToken);
            return client;
        }

        public static HttpContent CreateBodyContent(object body)
        {
            string dataJson = JsonConvert.SerializeObject(body);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");
            return data;
        }
    }
}
