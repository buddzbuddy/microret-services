using api.Resources;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

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
            var application = ApplicationHelper.CreateApplication();
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

        [Fact]
        public async Task CreateApplication_WhenInvoked_Returns400_IllegalPaymentTypeCodeError()
        {
            //Arrange
            var application = ApplicationHelper.CreateApplication();
            var client = application.CreateHttpClientJson();
            var nullData = new { };
            var paymentTypeCode = "SOME_CODE";
            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent(nullData));

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
            var responseObj = JsonConvert.DeserializeAnonymousType(responseBody, _responseType);
            responseObj?.errors[0].message.Should()
                .StartWith(ErrorMessageResource.IllegalDataProvidedError);
            responseObj?.errors[0].message.Should().Contain(nameof(paymentTypeCode));
        }

        [Fact]
        public async Task CreateApplication_WhenInvoked_Returns400_JsonEmptyError()
        {
            //Arrange
            var application = ApplicationHelper.CreateApplication();
            var client = application.CreateHttpClientJson();
            var paymentTypeCode = StaticReferences.PAYMENT_TYPE_UBK;

            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent(""));

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
            var responseObj = JsonConvert.DeserializeAnonymousType(responseBody, _responseType);
            responseObj?.errors[0].message.Should()
                .StartWith(ErrorMessageResource.JsonEmptyError);
        }

        [Fact]
        public async Task CreateApplication_WhenInvoked_Returns400_JsonInvalidError()
        {
            //Arrange
            var application = ApplicationHelper.CreateApplication();
            var client = application.CreateHttpClientJson();
            var paymentTypeCode = StaticReferences.PAYMENT_TYPE_UBK;

            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent("123"));

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
            var responseObj = JsonConvert.DeserializeAnonymousType(responseBody, _responseType);
            responseObj?.errors[0].message.Should()
                .StartWith(ErrorMessageResource.JsonInvalidError);
        }

        [Fact]
        public async Task CreateApplication_WhenInvoked_Returns400_JsonInvalidError()
        {
            //Arrange
            var application = ApplicationHelper.CreateApplication();
            var client = application.CreateHttpClientJson();
            var paymentTypeCode = StaticReferences.PAYMENT_TYPE_UBK;
            var json = JsonStorage.Get("InputApplicationModel.json");
            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent("123"));

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
            var responseObj = JsonConvert.DeserializeAnonymousType(responseBody, _responseType);
            responseObj?.errors[0].message.Should()
                .StartWith(ErrorMessageResource.JsonInvalidError);
        }
    }
}
