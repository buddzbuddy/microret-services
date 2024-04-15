using api.Contracts.BL.UBK;
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

namespace api.Tests.Systems.Services
{
    public class UbkVerifierTests : TestUtils
    {
        public UbkVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifySrcJson_WhenCalled_ThrowsEmptyError()
        {
            //Arrange
            var json = "";
            IUbkVerifier sut = new UbkVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifySrcJson(json));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonEmptyError);
        }

        [Fact]
        public void VerifySrcJson_WhenCalled_ThrowsInvalidJsonError()
        {
            //Arrange
            var json = "asdadas";
            IUbkVerifier sut = new UbkVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifySrcJson(json));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonInvalidError);
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ThrowsObjectNullError()
        {
            //Arrange
            ubkInputJsonDTO? nullJson = null;
            IUbkVerifier sut = new UbkVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifyParsedJsonData(nullJson));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonObjectNullError);
        }
    }
}
