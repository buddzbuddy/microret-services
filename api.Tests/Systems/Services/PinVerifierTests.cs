using api.Contracts.BL.Verifiers;
using api.Resources;
using api.Services.BL.Verifiers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
{
    public class PinVerifierTests : TestUtils
    {
        public PinVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyPin_WhenCalled_ThrowsNullError()
        {
            //Arrange
            string? pin = null;
            IPinVerifier sut = new PinVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyPin(pin));

            ex.ParamName.Should().Be(nameof(pin));
            ex.Message.Should().Contain(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyPin_WhenCalled_ThrowsPinLengthError()
        {
            //Arrange
            string? pin = "1234567891012";
            string expectedErrorMsg = string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.PIN_LENGTH);
            IPinVerifier sut = new PinVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.VerifyPin(pin));

            ex.ParamName.Should().Be(nameof(pin));
            ex.Message.Should().Contain(expectedErrorMsg);
        }

        [Fact]
        public void VerifyPin_WhenCalled_ThrowsPinInvalidError()
        {
            //Arrange
            string? pin = "1234567891012 ";
            IPinVerifier sut = new PinVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.VerifyPin(pin));

            ex.ParamName.Should().Be(nameof(pin));
            ex.Message.Should().Contain(ErrorMessageResource.StringShouldContainOnlyDigitsError);
        }
    }
}
