using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Services.BL.CISSA;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static api.Tests.Helpers.ApplicationHelper;

namespace api.Tests.Helpers
{
    public static class ApplicationHelper
    {
        public static WebApplicationFactory<Program> CreateApplication() 
            => createApplicationDefault();
        static WebApplicationFactory<Program> createApplicationDefault()
            => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
            });
        public static WebApplicationFactory<Program>
            CreateApplication(MockServices mockServices)
        {
            var application = createApplicationDefault();
            application.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    foreach ((var interfaceType, var serviceMock) in mockServices.GetMocks())
                    {
                        services.Remove(services.First(d => d.ServiceType == interfaceType));
                        services.AddScoped(typeof(IInputJsonParser), (servProv) => serviceMock);
                    }
                });
            });
            return application;
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
