using api.Contracts.BL;
using api.Models.BL;
using FluentAssertions;
using Moq;

namespace api.Tests.Systems.Services
{
    public class CissaRefServiceTests
    {
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
        }
    }
}