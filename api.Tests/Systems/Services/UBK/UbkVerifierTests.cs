using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;
using api.Resources;
using api.Services.BL.UBK;
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

namespace api.Tests.Systems.Services.UBK
{
    public class UbkVerifierTests : TestUtils
    {
        public UbkVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ThrowsObjectNullError()
        {
            //Arrange
            ubkInputModelDTO? nullJson = null;
            IUbkVerifier sut = new UbkVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifyInputModel(nullJson));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonObjectNullError);
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ThrowsIDNullError()
        {
            //Arrange
            ubkInputModelDTO? nullJson = new();
            IUbkVerifier sut = new UbkVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

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
            ubkInputModelDTO? nullJson = new() { ID = 123 };
            IUbkVerifier sut = new UbkVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            sut.VerifyInputModel(nullJson);
        }
    }
}
