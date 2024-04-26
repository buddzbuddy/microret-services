using api.Contracts.BL;
using api.Domain;
using api.Resources;
using api.Services.BL;
using api.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services.BL
{
    public class InputJsonParserTests : TestUtils
    {
        public InputJsonParserTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyJson_WhenCalled_ThrowsEmptyError()
        {
            //Arrange
            var json = "";
            IInputJsonParser sut = new InputJsonParserImpl();

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifyJson(json));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonEmptyError);
        }

        [Fact]
        public void VerifyJson_WhenCalled_ThrowsInvalidJsonError()
        {
            //Arrange
            var json = "asdadas";
            IInputJsonParser sut = new InputJsonParserImpl();

            //Act
            var ex = Assert.Throws<DomainException>(() => sut.VerifyJson(json));

            //Assert
            ex.Message.Should().Be(ErrorMessageResource.JsonInvalidError);
        }
    }
}
