using api.Contracts.BL;
using api.Services.BL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace api.Tests.Helpers
{
    public static class ApplicationHelper
    {
        public static WebApplicationFactory<Program> GetWebApplication()
            => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureTestServices(services =>
                {
                    /*var options = new DbContextOptionsBuilder<ApiContext>()
                                    .UseSqlServer(SqlConnectionString)
                                    .Options;
                    services.AddSingleton(options);
                    services.AddSingleton<ApiContext>();
                    services.AddDbContext<RepositoryContext>(opts => opts.UseNpgsql(SqlConnectionString));*/
                    services.AddScoped<ICissaRefService, CissaRefServiceImpl>();
                });
            });
        public static WebApplicationFactory<Program> GetWebApplicationAsProd()
            => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
            });

        public static HttpClient CreateHttpClientJson(this WebApplicationFactory<Program> application)
        {
            var client = application.CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        public static HttpClient CreateHttpClientJson(this WebApplicationFactory<Program> application, string bearerToken)
        {
            var client = application.CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
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
