﻿using api.Contracts.BL.UBK;
using api.Contracts.BL.UBK.PropsValidations;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Models.Enums;
using api.Resources;
using api.Services.BL.UBK.PropsValidations;
using api.Services.BL.UBK;
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
using static api.Models.BL.InputModelDTO;

namespace api.Tests.Systems.Services.BL.UBK
{
    public class ValidateExcludingInputsTests : TestUtils
    {
        public ValidateExcludingInputsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Validate_WhenCalled_ThrowsNullError()
        {
            //Arrange
            PersonDetailsDTO? person = null;
            IValidateExcludingInputs sut = new ValidateExcludingInputsImpl(Mock.Of<IDataHelper>(),
                Mock.Of<IUnemployeeValidator>(), Mock.Of<ISFService>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.Validate(person));
            ex.ParamName.Should().Be(nameof(person));
        }

        [Fact]
        public void Validate_WhenCalled_ThrowsUnemploymentStatusNullError()
        {
            //Arrange
            var person = new PersonDetailsDTO();
            var dataHelperMock = new Mock<IDataHelper>();
            dataHelperMock.Setup(s =>
            s.CalcAgeFromPinForToday(It.IsAny<string>())).Returns(StaticReferences.MEN_RETIREMENT_AGE - 1);
            dataHelperMock.Setup(s =>
            s.GetGender(It.IsAny<string>())).Returns(GenderType.MALE);
            IValidateExcludingInputs sut = new ValidateExcludingInputsImpl(dataHelperMock.Object,
                Mock.Of<IUnemployeeValidator>(), Mock.Of<ISFService>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.Validate(person));
            ex.ParamName.Should().Be(nameof(person.UnemployedStatusInfo));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void Validate_WhenCalled_ThrowsUnemploymentStatusIncorrectError()
        {
            //Arrange
            var person = new PersonDetailsDTO() { UnemployedStatusInfo = new() };
            var dataHelperMock = new Mock<IDataHelper>();
            dataHelperMock.Setup(s =>
            s.CalcAgeFromPinForToday(It.IsAny<string>())).Returns(StaticReferences.MEN_RETIREMENT_AGE - 1);
            dataHelperMock.Setup(s =>
            s.GetGender(It.IsAny<string>())).Returns(GenderType.MALE);
            var unemployeeValidatorMock = new Mock<IUnemployeeValidator>();
            unemployeeValidatorMock.Setup(s => s.IsUnemployee(person.UnemployedStatusInfo))
                .Returns(false);

            IValidateExcludingInputs sut = new ValidateExcludingInputsImpl(dataHelperMock.Object,
                unemployeeValidatorMock.Object, Mock.Of<ISFService>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.Validate(person));
            ex.ParamName.Should().Be(nameof(person.UnemployedStatusInfo.Status));
            ex.Message.Should().StartWith(ErrorMessageResource.UnemployeeStatusIncorrectError);
        }

        [Fact]
        public void Validate_WhenCalled_ReturnsOK()
        {
            //Arrange
            var person = new PersonDetailsDTO()
            {
                UnemployedStatusInfo = new() { Status = StaticReferences.UNEMPLOYEE_STATUS_NAME }
            };
            var dataHelperMock = new Mock<IDataHelper>();
            dataHelperMock.Setup(s =>
            s.CalcAgeFromPinForToday(It.IsAny<string>())).Returns(StaticReferences.MEN_RETIREMENT_AGE - 1);
            dataHelperMock.Setup(s =>
            s.GetGender(It.IsAny<string>())).Returns(GenderType.MALE);
            var unemployeeValidatorMock = new Mock<IUnemployeeValidator>();
            unemployeeValidatorMock.Setup(s => s.IsUnemployee(person.UnemployedStatusInfo))
                .Returns(true);
            IValidateExcludingInputs sut = new ValidateExcludingInputsImpl(dataHelperMock.Object,
                unemployeeValidatorMock.Object, Mock.Of<ISFService>());

            //Act & Assert
            sut.Validate(person);
        }
    }
}
