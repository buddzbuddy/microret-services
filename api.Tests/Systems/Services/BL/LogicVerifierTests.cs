﻿using api.Contracts.BL;
using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;
using api.Resources;
using api.Services.BL;
using api.Services.BL.UBK;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services.BL
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
            InputModelDTO? inputModel = null;
            ILogicVerifier sut = new LogicVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyInputModel(inputModel, StaticReferences.PAYMENT_TYPE_UBK));

            //Assert
            ex.ParamName.Should().Be(nameof(inputModel));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ThrowsIDNullError()
        {
            //Arrange
            InputModelDTO? nullJson = new();
            ILogicVerifier sut = new LogicVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyInputModel(nullJson, StaticReferences.PAYMENT_TYPE_UBK));

            //Assert
            ex.ParamName.Should().Be(nameof(nullJson.ID));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyParsedJsonData_WhenCalled_ReturnsOK()
        {
            //Arrange
            InputModelDTO? nullJson = new() { ID = 123 };
            ILogicVerifier sut = new LogicVerifierImpl(Mock.Of<IPersonalIdentityVerifier>(), Mock.Of<IPropertyVerifier>());

            //Act
            sut.VerifyInputModel(nullJson, StaticReferences.PAYMENT_TYPE_UBK);
        }
    }
}
