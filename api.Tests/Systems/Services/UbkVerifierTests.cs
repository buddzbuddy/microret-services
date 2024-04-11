using api.Contracts.BL.UBK;
using api.Domain;
using api.Models.BL;
using api.Resources;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
        public void WhenCalled_VerifySrcJson_ThrowsEmptyError()
        {
            //Arrange
            var json = "";
            var application = ApplicationHelper.GetWebApplication();
            using var scope = application.Services.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IUbkVerifier>();

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifySrcJson(json));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonEmptyError);
        }

        [Fact]
        public void WhenCalled_VerifySrcJson_ThrowsInvalidJsonError()
        {
            //Arrange
            var json = "asdadas";
            var application = ApplicationHelper.GetWebApplication();
            using var scope = application.Services.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IUbkVerifier>();

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifySrcJson(json));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonInvalidError);
        }

        [Fact]
        public void WhenCalled_VerifySrcJson_ThrowsObjectNullError()
        {
            //Arrange
            ubkInputJsonDTO? nullJson = null;
            var application = ApplicationHelper.GetWebApplication();
            using var scope = application.Services.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IUbkVerifier>();

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifyParsedJsonData(nullJson));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonObjectNullError);
        }

        [Fact]
        public void WhenCalled_VerifySrcJson_ThrowsApplicantNullError()
        {
            //Arrange
            ubkInputJsonDTO? nullJson = new();
            var application = ApplicationHelper.GetWebApplication();
            using var scope = application.Services.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IUbkVerifier>();

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyParsedJsonData(nullJson));

            //Assert
            ex.ParamName.Should().Be(nameof(nullJson.Applicant));
        }

        [Fact]
        public void WhenCalled_VerifySrcJson_ThrowsFamilyMembersNullOrEmptyError()
        {
            //Arrange
            ubkInputJsonDTO? nullJson = new() { Applicant = new ()};
            var application = ApplicationHelper.GetWebApplication();
            using var scope = application.Services.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IUbkVerifier>();

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyParsedJsonData(nullJson));

            //Assert
            ex.ParamName.Should().Be(nameof(nullJson.FamilyMembers));
        }

        [Fact]
        public void WhenCalled_VerifySrcJson_Returns_OK_Empty()
        {
            //Arrange
            var json = @"{""t1"":123}";
            var application = ApplicationHelper.GetWebApplication();
            using var scope = application.Services.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IUbkVerifier>();

            //Act
            sut.VerifySrcJson(json);

            //Assert
        }
    }
}
