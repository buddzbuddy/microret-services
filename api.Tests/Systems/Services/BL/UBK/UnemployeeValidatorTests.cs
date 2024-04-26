using api.Contracts.BL.UBK.PropsValidations;
using api.Models.BL;
using api.Services.BL.UBK.PropsValidations;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services.BL.UBK
{
    public class UnemployeeValidatorTests : TestUtils
    {
        public UnemployeeValidatorTests(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public void IsUnemployee_WhenCalledOnNull_ReturnsFalse()
        {
            //Arrange
            bool expectedResult = false;
            UnemployedStatusInfoDTO? unemployedStatus = null;
            IUnemployeeValidator sut = new UnemployeeValidatorImpl();

            //Act
            var result = sut.IsUnemployee(unemployedStatus);

            //Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void IsUnemployee_WhenCalled_ReturnsFalse()
        {
            //Arrange
            bool expectedResult = false;
            UnemployedStatusInfoDTO? unemployedStatus = new() { Status = "some other status" };
            IUnemployeeValidator sut = new UnemployeeValidatorImpl();

            //Act
            var result = sut.IsUnemployee(unemployedStatus);

            //Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void IsUnemployee_WhenCalled_ReturnsTrue()
        {
            //Arrange
            bool expectedResult = true;
            UnemployedStatusInfoDTO? unemployedStatus =
                new() { Status = StaticReferences.UNEMPLOYEE_STATUS_NAME };
            IUnemployeeValidator sut = new UnemployeeValidatorImpl();

            //Act
            var result = sut.IsUnemployee(unemployedStatus);

            //Assert
            result.Should().Be(expectedResult);
        }
    }
}
