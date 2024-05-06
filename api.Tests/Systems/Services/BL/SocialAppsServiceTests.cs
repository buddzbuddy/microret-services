using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Resources;
using api.Services.BL;
using api.Services.BL.UBK;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services.BL
{
    public class SocialAppsServiceTests : TestUtils
    {
        public SocialAppsServiceTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateApplication_WhenCalled_Returns_OK_Empty()
        {
            //Arrange
            var expectedResult = ("123", Guid.NewGuid());
            var json_data = @"
{
""t1"":123,""t2"":""123""
}
";
            var dataSvc = Mock.Of<IDataService>();
            var verifier = Mock.Of<ILogicVerifier>();
            var dataParserMock = new Mock<IInputJsonParser>();
            dataParserMock.Setup(s => s.ParseToModel<InputModelDTO>(json_data)).Returns(new InputModelDTO());
            var mockCissaDataProvider = new Mock<ICissaDataProvider>();
            mockCissaDataProvider.Setup(s =>
            s.CreateCissaApplication(It.IsAny<PersonDetailsDTO>(),
            StaticCissaReferences.PAYMENT_TYPE_UBK)).ReturnsAsync(expectedResult);
            ISocialAppsService sut = new SocialAppsServiceImpl(dataSvc, verifier, dataParserMock.Object,
                mockCissaDataProvider.Object, Mock.Of<IHttpService>());

            //Act
            var result = await sut.CreateApplication(json_data, StaticReferences.PAYMENT_TYPE_UBK);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CreateApplication_WhenCalled_ThrowsIllegalPaymentException()
        {
            //Arrange
            var json_data = @"
{
""t1"":123,""t2"":""123""
}
";
            string paymentTypeCode = It.IsAny<string>();
            var dataSvc = Mock.Of<IDataService>();
            var verifier = Mock.Of<ILogicVerifier>();
            var dataParser = Mock.Of<IInputJsonParser>();
            var cissaDataProvider = Mock.Of<ICissaDataProvider>();
            ISocialAppsService sut = new SocialAppsServiceImpl(dataSvc, verifier,
                dataParser, cissaDataProvider, Mock.Of<IHttpService>());

            //Act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>
                (async () => await sut.CreateApplication(json_data, paymentTypeCode));

            //Assert
            ex.ParamName.Should().Be(nameof(paymentTypeCode));
            ex.Message.Should().StartWith(ErrorMessageResource.IllegalDataProvidedError);
        }

        [Fact]
        public async Task SetApplicationResult_WhenCalled_ThrowsNullException()
        {
            //Arrange
            setApplicationResultDTO? dto = null;
            var dataSvc = Mock.Of<IDataService>();
            var verifier = Mock.Of<ILogicVerifier>();
            var dataParser = Mock.Of<IInputJsonParser>();
            var cissaDataProvider = Mock.Of<ICissaDataProvider>();
            ISocialAppsService sut = new SocialAppsServiceImpl(dataSvc, verifier, dataParser,
                cissaDataProvider, Mock.Of<IHttpService>());

            //Act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>
                (async () => await sut.SetApplicationResult(dto));

            //Assert
            ex.ParamName.Should().Be(nameof(dto));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public async Task SetApplicationResult_WhenCalled_ThrowsPropsNullException()
        {
            //Arrange
            setApplicationResultDTO? dto = new();
            var dataSvc = Mock.Of<IDataService>();
            var verifier = Mock.Of<ILogicVerifier>();
            var dataParser = Mock.Of<IInputJsonParser>();
            var cissaDataProvider = Mock.Of<ICissaDataProvider>();
            ISocialAppsService sut = new SocialAppsServiceImpl(dataSvc, verifier, dataParser,
                cissaDataProvider, Mock.Of<IHttpService>());

            //Act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>
                (async () => await sut.SetApplicationResult(dto));

            //Assert
            ex.ParamName.Should().Be("appId,Decision");
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }
    }
}
