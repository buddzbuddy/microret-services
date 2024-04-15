using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Services.BL.UBK;
using api.Tests.Helpers;
using api.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
{
    public class PersonalIdentityVerifierTests : TestUtils
    {
        public PersonalIdentityVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyApplicant_WhenCalled_ThrowsApplicantNullError()
        {
            //Arrange
            ubkInputJsonDTO.ApplicantDTO? applicant = null;
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), Mock.Of<IDataHelper>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyApplicant(applicant));

            //Assert
            ex.ParamName.Should().Be(nameof(applicant));
        }

        [Fact]
        public void VerifyApplicant_WhenCalled_ThrowsFactAddressNullError()
        {
            //Arrange
            ubkInputJsonDTO.ApplicantDTO applicant = new() { };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), Mock.Of<IDataHelper>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyApplicant(applicant));

            //Assert
            ex.ParamName.Should().Be("address");
        }

        [Fact]
        public void VerifyApplicant_WhenCalled_ThrowsFactAddressRegionNullError()
        {
            //Arrange
            ubkInputJsonDTO.ApplicantDTO applicant = new() { ResidentialAddress =
                new ResidentialAddressDTO { }
            };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), Mock.Of<IDataHelper>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyApplicant(applicant));

            //Assert
            ex.ParamName.Should().Contain("Region");
            _output.WriteLine(ex.ParamName);
        }

        [Fact]
        public void VerifyFamilyMembers_WhenCalled_ThrowsFamilyMembersNullError()
        {
            //Arrange
            ubkInputJsonDTO.FamilyMemberDTO[]? familyMembers = null;
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), Mock.Of<IDataHelper>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            ex.ParamName.Should().Be(nameof(familyMembers));
        }

        [Fact]
        public void VerifyFamilyMembers_WhenCalled_ThrowsFamilyMemberRoleNullError()
        {
            //Arrange
            var familyMembers = new ubkInputJsonDTO.FamilyMemberDTO[]
            { new() { pin = "123", lastname = "some", firstname = "some", patronymic = "some",
                roleId = 123 } };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), Mock.Of<IDataHelper>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            var fMemItem = familyMembers[0];
            ex.ParamName.Should().Be(nameof(fMemItem.role));
        }

        [Fact]
        public void VerifyFamilyMembers_WhenCalled_ThrowsFamilyMemberRoleIdNullError()
        {
            //Arrange
            var familyMembers = new ubkInputJsonDTO.FamilyMemberDTO[]
            { new() { pin = "123", lastname = "some", firstname = "some", patronymic = "some",
                role = "some" } };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), Mock.Of<IDataHelper>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            var fMemItem = familyMembers[0];
            ex.ParamName.Should().Be(nameof(fMemItem.roleId));
        }

        [Fact]
        public void VerifyFamilyMembers_WhenCalled_ThrowsFamilyMemberBirthActNullError()
        {
            //Arrange
            var pin = "123";
            var familyMembers = new ubkInputJsonDTO.FamilyMemberDTO[]
            { new() { pin = pin, lastname = "some", firstname = "some", patronymic = "some",
                role = "some", roleId = 123 } };

            var dataHelperMock = new Mock<IDataHelper>();
            dataHelperMock.Setup(s => s.CalcAgeFromPinForToday(pin)).Returns(17);
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), dataHelperMock.Object);

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            ex.ParamName.Should().Be("birthAct");
        }

        [Fact]
        public void VerifyFamilyMembers_WhenCalled_ThrowsBirthActPropsNullError()
        {
            //Arrange
            var pin = "123";
            var familyMembers = new ubkInputJsonDTO.FamilyMemberDTO[]
            { new() { pin = pin, lastname = "some", firstname = "some", patronymic = "some",
                role = "some", roleId = 123, BirthActByPinInfo = new() } };

            var dataHelperMock = new Mock<IDataHelper>();
            dataHelperMock.Setup(s => s.CalcAgeFromPinForToday(pin)).Returns(17);
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>(), dataHelperMock.Object);

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            ex.ParamName.Should().Be("ActDate,ActNumber,ActGovUnit,ChildSurname,ChildFirstName," +
                "ChildGender,ChildPlaceOfBirth,MotherPin,MotherSurname,MotherFirstName," +
                "MotherNationality,MotherCitizenship");
        }
    }
}
