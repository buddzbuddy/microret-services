using api.Contracts.BL.CISSA;
using api.Domain;
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

        [Fact]
        public async Task GetGMI_WhenCalled_WithWrongInputs_Throws_NoData()
        {
            //Arrange
            string expectedErrorMessage = "Wrong input data";
            int year = DateTime.Today.Year + 2; int month = 1;
            var requestDTO = new gmiRequestDTO { year = year, month = month };
            var cissaRefSvcMock = new Mock<ICissaRefService>();
            cissaRefSvcMock.Setup(svc => svc.GetGMI(requestDTO)).ThrowsAsync(new DomainException(expectedErrorMessage));

            //Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(async () => await cissaRefSvcMock.Object.GetGMI(requestDTO));
            ex.Message.Should().Be(expectedErrorMessage);
            _output.WriteLine(ex.Message);
        }
    }
}