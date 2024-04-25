using api.Contracts.Helpers;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.Services.Helpers
{
    public class HttpServiceImpl : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _microretConf;
        public HttpServiceImpl(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _microretConf = _configuration.GetSection("MICRORET");
        }
        public async Task CallChangeDecision(int id, string decision, string rejectionReason)
        {
            var token = await GetAccessToken();
            var request = new
            {
                RejectionReason = rejectionReason,
                Decision = decision,
            };
            var _responseType = new { message = (string?)"", error = new { Error = (string?)"", Code = (int?)0 } };
            var result = await sendHttpRequest(_responseType, $"{_microretConf.GetValue<string>("changeDecisionUri")!}{id}", "PATCH", request, token);
            if(result != null)
            {
                if (!string.IsNullOrEmpty(result.message))
                {
                    return;
                }
                if(result.error != null && !string.IsNullOrEmpty(result.error.Error))
                {
                    throw new Domain.DomainException(result.error.Error, result.error.Code!.Value.ToString() );
                }
            }
        }

        public async Task<string> GetAccessToken()
        {
            var body = new
            {
                username = _microretConf.GetValue<string>("username"),
                password = _microretConf.GetValue<string>("password")
            };
            var _responseType = new { AccessToken = (string?)"" };
            var loginResult = await sendHttpRequest(_responseType, _microretConf.GetValue<string>("loginUri")!, "POST", body);
            if (loginResult != null)
            {
                return loginResult.AccessToken ?? throw new ArgumentNullException(nameof(loginResult.AccessToken), ErrorMessageResource.NullDataProvidedError);
            }
            else
                throw new ArgumentNullException(nameof(loginResult), ErrorMessageResource.NullDataProvidedError);
        }

        private async Task<T?> sendHttpRequest<T>(T responseType, string uri, string method, object? body = null, string? accessToken = null)
        {
            var _uri = new Uri(uri);
            var req = new HttpRequestMessage();
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(accessToken))
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            req.Method = new HttpMethod(method);
            req.RequestUri = _uri;
            if (body != null)
                req.Content = CreateBodyContent(body);
            var result = await _httpClient.SendAsync(req);
            //result.EnsureSuccessStatusCode();
            var jsonStr = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeAnonymousType(jsonStr, responseType);
        }

        private HttpContent CreateBodyContent(object body)
        {
            string dataJson = JsonConvert.SerializeObject(body);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");
            return data;
        }

    }
}
