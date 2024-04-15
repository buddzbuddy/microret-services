using api.Contracts.BL.Verifiers;
using api.Models.BL;
using api.Resources;
using api.Services.BL.Verifiers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
{
    public class PassportDataVerifierTests : TestUtils
    {
        public PassportDataVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyPassportData_WhenCalled_ThrowsNullError()
        {
            //Arrange
            PassportDataInfoDTO? passport = null;
            IPassportDataVerifier sut = new PassportDataVerifierImpl(Mock.Of<IPinVerifier>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyPassportData(passport));

            ex.ParamName.Should().Be(nameof(passport));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyPassportData_WhenCalled_ThrowsRequiredPropsNullError()
        {
            //Arrange
            PassportDataInfoDTO? passport = new();
            IPassportDataVerifier sut = new PassportDataVerifierImpl(Mock.Of<IPinVerifier>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyPassportData(passport));

            ex.ParamName.Should().Be("PassportSeries,PassportNumber,PassportAuthority,IssuedDate");
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyPassportData_WhenCalled_ThrowsIssueDateInvaildError()
        {
            //Arrange
            PassportDataInfoDTO? passport = new()
            {
                PassportSeries = "AN",
                PassportNumber = "1234567",
                PassportAuthority = "some authority",
                IssuedDate = DateTime.Today.AddDays(1)
            };
            IPassportDataVerifier sut = new PassportDataVerifierImpl(Mock.Of<IPinVerifier>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.VerifyPassportData(passport));

            ex.ParamName.Should().Be(nameof(passport.IssuedDate));
            ex.Message.Should().StartWith(ErrorMessageResource.IllegalDataProvidedError);
        }

        [Fact]
        public void VerifyPassportData_WhenCalled_ThrowsExceededIssueDateError()
        {
            //Arrange
            PassportDataInfoDTO? passport = new()
            {
                PassportSeries = "AN",
                PassportNumber = "1234567",
                PassportAuthority = "some authority",
                IssuedDate = DateTime.Today.AddYears(-StaticReferences.PASSPORT_DEFAULT_VALID_YEARS)
                .AddYears(-1)
            };
            _output.WriteLine($"passport:\n{JsonConvert.SerializeObject(passport)}");
            IPassportDataVerifier sut = new PassportDataVerifierImpl(Mock.Of<IPinVerifier>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.VerifyPassportData(passport));

            ex.ParamName.Should().Be(nameof(passport.IssuedDate));
            ex.Message.Should().StartWith(ErrorMessageResource.PassportExpiredError);
        }

        [Fact]
        public void VerifyPassportData_WhenCalled_Succeeded()
        {
            //Arrange
            PassportDataInfoDTO? passport = new()
            {
                PassportSeries = "AN",
                PassportNumber = "1234567",
                PassportAuthority = "some authority",
                IssuedDate = DateTime.Today.AddDays(1)
                .AddYears(-StaticReferences.PASSPORT_DEFAULT_VALID_YEARS)
            };
            IPassportDataVerifier sut = new PassportDataVerifierImpl(Mock.Of<IPinVerifier>());

            //Act
            sut.VerifyPassportData(passport);
        }
    }
}
