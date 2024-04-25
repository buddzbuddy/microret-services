using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.BL.ESP;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Services.BL.ESP;
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

namespace api.Tests.Systems.Services.ESP
{
    public class EspServiceTests : TestUtils
    {
        public EspServiceTests(ITestOutputHelper output) : base(output)
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
            var dataSvc = Mock.Of<IEspDataService>();
            var verifier = Mock.Of<IEspVerifier>();
            var dataParserMock = new Mock<IInputJsonParser>();
            dataParserMock.Setup(s => s.ParseToModel<espInputModelDTO>(json_data)).Returns(new espInputModelDTO());
            var mockCissaDataProvider = new Mock<ICissaDataProvider>();
            mockCissaDataProvider.Setup(s =>
            s.CreateCissaApplication(It.IsAny<PersonDetailsDTO>(), null)).ReturnsAsync(expectedResult);
            IEspService sut = new EspServiceImpl(dataSvc, dataParserMock.Object, verifier,
                mockCissaDataProvider.Object, Mock.Of<IHttpService>());

            //Act
            var result = await sut.CreateApplication(json_data);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
