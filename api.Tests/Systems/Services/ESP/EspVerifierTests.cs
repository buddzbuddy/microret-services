using api.Contracts.BL.ESP;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;
using api.Resources;
using api.Services.BL.ESP;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using FluentAssertions;
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
    public class EspVerifierTests : TestUtils
    {
        public EspVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ThrowsObjectNullError()
        {
            //Arrange
            espInputModelDTO? nullJson = null;
            IEspVerifier sut = new EspVerifierImpl(Mock.Of<IPersonalIdentityVerifier>());

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifyInputModel(nullJson));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonObjectNullError);
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ThrowsIDNullError()
        {
            //Arrange
            espInputModelDTO? nullJson = new();
            IEspVerifier sut = new EspVerifierImpl(Mock.Of<IPersonalIdentityVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyInputModel(nullJson));

            //Assert
            ex.ParamName.Should().Be(nameof(nullJson.ID));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ReturnsOK()
        {
            //Arrange
            espInputModelDTO? nullJson = new() { ID = 123 };
            IEspVerifier sut = new EspVerifierImpl(Mock.Of<IPersonalIdentityVerifier>());

            //Act
            sut.VerifyInputModel(nullJson);
        }
    }
}
