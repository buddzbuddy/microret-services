using api.Contracts.BL.Verifiers;
using api.Contracts.Helpers;
using api.Models.Enums;
using api.Resources;
using api.Services.BL.Verifiers;
using api.Services.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
{
    public class DataHelperTests : TestUtils
    {
        public DataHelperTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CalcAgeForToday_WhenCalled_ThrowsNullError()
        {
            //Arrange
            DateTime? birthDate = null;
            IDataHelper sut = new DataHelperImpl(Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.CalcAgeForToday(birthDate));

            //Assert
            ex.ParamName.Should().Be(nameof(birthDate));
            ex.Message.Should().Contain(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void CalcAgeForToday_WhenCalled_ThrowsIllegalError()
        {
            //Arrange
            var birthDate = DateTime.Today.AddDays(1);
            IDataHelper sut = new DataHelperImpl(Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentException>(() => sut.CalcAgeForToday(birthDate));

            //Assert
            ex.ParamName.Should().Be(nameof(birthDate));
            ex.Message.Should().Contain(ErrorMessageResource.IllegalDataProvidedError);
        }

        [Fact]
        public void CalcAgeForToday_WhenCalled_ReturnsAge()
        {
            //Arrange
            var expectedAge = 12;
            var birthDate = DateTime.Today.AddYears(-expectedAge);
            IDataHelper sut = new DataHelperImpl(Mock.Of<IPinVerifier>());

            //Act
            var result = sut.CalcAgeForToday(birthDate);

            //Assert
            result.Should().Be(expectedAge);
        }

        [Fact]
        public void GetDate_WhenCalled_ThrowsNullError()
        {
            //Arrange
            var dateStr = "";
            IDataHelper sut = new DataHelperImpl(Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.GetDate(dateStr));

            //Assert
            ex.ParamName.Should().Be(nameof(dateStr));
            ex.Message.Should().Contain(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void GetDate_WhenCalled_ThrowsFormatError()
        {
            //Arrange
            var dateStr = "01012020";
            IDataHelper sut = new DataHelperImpl(Mock.Of<IPinVerifier>());

            //Act & Assert
            Assert.Throws<FormatException>(() => sut.GetDate(dateStr));
        }

        [Fact]
        public void GetDate_WhenCalled_ReturnsDate()
        {
            //Arrange
            var dateStr = "01012020";
            var expectedDate = new DateTime(2020, 1, 1);
            IDataHelper sut = new DataHelperImpl(Mock.Of<IPinVerifier>());

            //Act
            var result = sut.GetDate(dateStr, "ddMMyyyy");
            //Assert
            result.Should().Be(expectedDate);
        }

        [Fact]
        public void ExtractBirthDate_WhenCalled_ReturnsDate()
        {
            //Arrange
            var pin = "10101202012345";
            var expectedDate = new DateTime(2020, 1, 1);
            IDataHelper sut = new DataHelperImpl(new PinVerifierImpl());

            //Act
            var result = sut.ExtractBirthDate(pin);
            //Assert
            result.Should().Be(expectedDate);
        }

        [Fact]
        public void CalcAgeFromPinForToday_WhenCalled_ReturnsAge()
        {
            //Arrange
            var expectedAge = 4;
            var pin = "10101202012345";
            IDataHelper sut = new DataHelperImpl(new PinVerifierImpl());

            //Act
            var result = sut.CalcAgeFromPinForToday(pin);
            //Assert
            result.Should().Be(expectedAge);
        }

        [Fact]
        public void GetGender_WhenCalled_ReturnsMale()
        {
            //Arrange
            var expectedGender = GenderType.MALE;
            var pin = "20101202012345";
            IDataHelper sut = new DataHelperImpl(new PinVerifierImpl());

            //Act
            var result = sut.GetGender(pin);
            //Assert
            result.Should().Be(expectedGender);
        }
    }
}
