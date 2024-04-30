using api.Resources;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.Tests.Systems.Apis.V1.Controllers
{
    public class socialAppsControllerTests : TestUtils
    {
        public socialAppsControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task SetResult_WhenInvoked_Returns400_PropsNullError()
        {
            //Arrange
            var application = ApplicationHelper.GetWebApplication();
            var client = application.CreateHttpClientJson();
            var data = new { };

            //Act
            var response = await client.PostAsync("api/v1/social-apps/set-result",
                    ApplicationHelper.CreateBodyContent(data));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            var _responseType = new
            {
                errors = new[]
                {
                    new
                    {
                        message = string.Empty
                    }
                }
            };
            _output.WriteLine(responseBody);
            _output.WriteLine(JsonConvert.SerializeObject(_responseType));
            var responseObj = JsonConvert.DeserializeAnonymousType(responseBody, _responseType);
            responseObj?.errors[0].message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
            responseObj?.errors[0].message.Should().Contain("appId,Decision");
        }
    }
}
