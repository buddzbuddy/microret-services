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
    public class PersonDataVerifierTests : TestUtils
    {
        public PersonDataVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsLastNameNullError()
        {
            //Arrange
            string? surname = null;
            string? name = null;
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(surname));
            ex.Message.Should().Contain(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsLastNameMinimumLengthError()
        {
            //Arrange
            string expectedErrorMsg = string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.MINIMUM_NAME_LENGTH);
            string? surname = "s";
            string? name = null;
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(surname));
            ex.Message.Should().StartWith(expectedErrorMsg);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsLastNameLettersOnlyError()
        {
            //Arrange
            string? surname = "some 2";
            string? name = null;
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(surname));
            ex.Message.Should().StartWith(ErrorMessageResource.StringShouldContainOnlyLettersError);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsFirstNameNullError()
        {
            //Arrange
            string? surname = "some";
            string? name = null;
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(name));
            ex.Message.Should().Contain(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsFirstNameMinimumLengthError()
        {
            //Arrange
            string expectedErrorMsg = string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.MINIMUM_NAME_LENGTH);
            string? surname = "some";
            string? name = "s";
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(name));
            ex.Message.Should().StartWith(expectedErrorMsg);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsFirstNameLettersOnlyError()
        {
            //Arrange
            string? surname = "some";
            string? name = "some 2";
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(name));
            ex.Message.Should().StartWith(ErrorMessageResource.StringShouldContainOnlyLettersError);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsPatronymicIllegalError()
        {
            //Arrange
            string? surname = "some " + StaticReferences.MALE_ENDS_WITH;
            string? name = "some";
            string? patronymic = "some";
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(patronymic));
            ex.Message.Should().StartWith(ErrorMessageResource.IllegalDataProvidedError);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsPatronymicNullError()
        {
            //Arrange
            string? surname = "some";
            string? name = "some";
            string? patronymic = null;
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(patronymic));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsPatronymicMinLengthError()
        {
            //Arrange
            string expectedErrorMsg = string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.MINIMUM_NAME_LENGTH);
            string? surname = "some";
            string? name = "some";
            string? patronymic = "s";
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(patronymic));
            ex.Message.Should().StartWith(expectedErrorMsg);
        }

        [Fact]
        public void WhenCalled_VerifyNames_ThrowsPatronymicOnlyLettersError()
        {
            //Arrange
            string expectedErrorMsg = string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.MINIMUM_NAME_LENGTH);
            string? surname = "some";
            string? name = "some";
            string? patronymic = "some 2";
            IPersonDataVerifier sut = new PersonDataVerifierImpl();

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            sut.VerifyNames(surname, name, patronymic));

            ex.ParamName.Should().Be(nameof(patronymic));
            ex.Message.Should().StartWith(ErrorMessageResource.StringShouldContainOnlyLettersError);
        }
    }
}
