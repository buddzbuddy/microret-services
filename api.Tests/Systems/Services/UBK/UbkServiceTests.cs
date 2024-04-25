using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.BL.UBK;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Services.BL.UBK;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services.UBK
{
    public class UbkServiceTests : TestUtils
    {
        public UbkServiceTests(ITestOutputHelper output) : base(output)
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
            var dataSvc = Mock.Of<IUbkDataService>();
            var verifier = Mock.Of<IUbkVerifier>();
            var dataParserMock = new Mock<IInputJsonParser>();
            dataParserMock.Setup(s => s.ParseToModel<ubkInputModelDTO>(json_data)).Returns(new ubkInputModelDTO());
            var mockCissaDataProvider = new Mock<ICissaDataProvider>();
            mockCissaDataProvider.Setup(s =>
            s.CreateCissaApplication(It.IsAny<PersonDetailsDTO>(),
            StaticCissaReferences.PAYMENT_TYPE_UBK)).ReturnsAsync(expectedResult);
            IUbkService sut = new UbkServiceImpl(dataSvc, verifier, dataParserMock.Object,
                mockCissaDataProvider.Object, Mock.Of<IHttpService>());

            //Act
            var result = await sut.CreateApplication(json_data);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
