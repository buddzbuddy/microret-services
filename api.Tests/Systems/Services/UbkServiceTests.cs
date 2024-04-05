using api.Contracts.BL.UBK;
using api.Services.BL.UBK;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
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
            var expectedResult = 123;
            var json_data = @"
{
""t1"":123,""t2"":""123""
}
";
            var mockDataSvc = new Mock<IUbkDataService>();
            mockDataSvc.Setup(s => s.InsertSrcJsonToDb(json_data)).ReturnsAsync(expectedResult);
            var mockVerifier = new Mock<IUbkVerifier>();
            mockVerifier.Setup(s => s.VerifySrcJson(json_data));
            IUbkService sut = new UbkServiceImpl(mockDataSvc.Object, mockVerifier.Object);

            //Act
            var result  = await sut.CreateApplication(json_data);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
