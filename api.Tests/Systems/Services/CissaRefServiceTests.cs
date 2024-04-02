using api.Contracts.BL;
using api.Models.BL;
using api.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
{
    public class CissaRefServiceTests : TestUtils
    {
        public CissaRefServiceTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetGMI_WhenCalled_Returns_Data()
        {
            //Arrange
            double expectedResult = 1000;
            int year = 2024; int month = 1;
            var requestDTO = new gmiRequestDTO { year = year, month = month };
            var cissaRefSvcMock = new Mock<ICissaRefService>();
            cissaRefSvcMock.Setup(svc => svc.GetGMI(requestDTO)).ReturnsAsync(expectedResult);

            //Act
            var result = await cissaRefSvcMock.Object.GetGMI(requestDTO);

            //Assert
            result.Should().Be(expectedResult);
            _output.WriteLine(result.ToString());
        }
    }
}