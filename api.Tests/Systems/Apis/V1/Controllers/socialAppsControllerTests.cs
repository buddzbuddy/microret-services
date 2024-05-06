using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.BL.Verifiers;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Resources;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using System.Net;
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
        public async Task CreateApplication_WhenInvoked_Returns400_2_JsonEmptyError()
        {
            //Arrange
            var json = JsonStorage.Get("EmptyInputApplicationModel.json");
            _output.WriteLine(json);
            var inputJsonParserMock = new Mock<IInputJsonParser>();
            inputJsonParserMock.Setup(svc => svc.ParseToModel<InputModelDTO>(json)).Returns(new InputModelDTO());
            var application = ApplicationHelper.CreateApplication();
            //application.MockService(inputJsonParserMock);
            var client = application.CreateHttpClientJson();
            var paymentTypeCode = StaticReferences.PAYMENT_TYPE_UBK;
            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent(json));

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
        public async Task CreateApplication_WhenInvoked_Returns400_ID_NullError()
        {
            //Arrange
            var json = JsonStorage.Get("ID_NULL_InputApplicationModel.json");
            var inputJsonParserMock = new Mock<IInputJsonParser>();
            inputJsonParserMock.Setup(svc => svc.ParseToModel<InputModelDTO>(json)).Returns(new InputModelDTO());
            var application = ApplicationHelper.CreateApplication();
            //application.MockService(inputJsonParserMock);
            var client = application.CreateHttpClientJson();
            var paymentTypeCode = StaticReferences.PAYMENT_TYPE_UBK;
            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent(json));

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
                .StartWith(ErrorMessageResource.NullDataProvidedError);
            responseObj?.errors[0].message.Should()
                .Contain("ID");
        }

        [Fact]
        public async Task CreateApplication_WhenInvoked_ReturnsSuccess()
        {
            //Arrange
            var json = JsonStorage.Get("ValidInputApplicationModel.json");
            var newPkgId = 123;
            var regNo = "123";
            var appId = Guid.NewGuid();
            var dataSvcMock = new Mock<IDataService>();
            dataSvcMock.Setup(svc => svc.SaveJson(It.IsAny<string>())).ReturnsAsync(newPkgId);
            var cissaDataProviderMock = new Mock<ICissaDataProvider>();
            cissaDataProviderMock.Setup(svc => svc.CreateCissaApplication(It.IsAny<PersonDetailsDTO>(), StaticCissaReferences.PAYMENT_TYPE_UBK)).ReturnsAsync((regNo, appId));
            dataSvcMock.Setup(svc => svc.UpdatePackageInfo(newPkgId, regNo, appId));
            
            var application = ApplicationHelper.CreateApplicationMock(
                new()
                {
                    (typeof(ILogicVerifier), Mock.Of<ILogicVerifier>()),
                    (typeof(IDataService), dataSvcMock.Object),
                    (typeof(ICissaDataProvider), cissaDataProviderMock.Object),
                });

            var client = application.CreateHttpClientJson();
            var paymentTypeCode = StaticReferences.PAYMENT_TYPE_UBK;
            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/send-application/{paymentTypeCode}",
                ApplicationHelper.CreateBodyContent(json));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<createApplicationResultDTO>(responseBody);
            responseObj?.appId.Should().Be(appId);
            responseObj?.regNo.Should().Be(regNo);
        }

        [Fact]
        public async Task SetResult_WhenInvoked_ReturnSuccess()
        {
            //Arrange
            var appId = new Guid("{39C0006F-90D8-4D00-85A3-F2443A1FA404}");
            var dataSvcMock = new Mock<IDataService>();
            dataSvcMock.Setup(svc => svc.GetOriginAppID(appId)).ReturnsAsync(123);

            var application = ApplicationHelper.CreateApplicationMock(
                new()
                {
                    (typeof(IDataService), dataSvcMock.Object),
                    (typeof(IHttpService), Mock.Of<IHttpService>())
                });
            var client = application.CreateHttpClientJson();
            var request = new setApplicationResultDTO
            {
                appId = appId,
                Decision = "some decision"
            };

            //Act
            var response = await client.PostAsync(
                $"api/v1/social-apps/set-result",
                ApplicationHelper.CreateBodyContent(request));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
