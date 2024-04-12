using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
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
        public void WhenCalled_VerifyApplicant_ThrowsApplicantNullError()
        {
            //Arrange
            ubkInputJsonDTO.ApplicantDTO? applicant = null;
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyApplicant(applicant));

            //Assert
            ex.ParamName.Should().Be(nameof(applicant));
        }

        [Fact]
        public void WhenCalled_VerifyApplicant_ThrowsFactAddressNullError()
        {
            //Arrange
            ubkInputJsonDTO.ApplicantDTO applicant = new() { };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyApplicant(applicant));

            //Assert
            ex.ParamName.Should().Be("address");
        }

        [Fact]
        public void WhenCalled_VerifyApplicant_ThrowsFactAddressRegionNullError()
        {
            //Arrange
            ubkInputJsonDTO.ApplicantDTO applicant = new() { ResidentialAddress =
                new ResidentialAddressDTO { }
            };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyApplicant(applicant));

            //Assert
            ex.ParamName.Should().Contain("Region");
            _output.WriteLine(ex.ParamName);
        }

        [Fact]
        public void WhenCalled_VerifyFamilyMembers_ThrowsFamilyMembersNullError()
        {
            //Arrange
            ubkInputJsonDTO.FamilyMemberDTO[]? familyMembers = null;
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            ex.ParamName.Should().Be(nameof(familyMembers));
        }

        [Fact]
        public void WhenCalled_VerifyFamilyMembers_ThrowsFamilyMemberRoleNullError()
        {
            //Arrange
            ubkInputJsonDTO.FamilyMemberDTO[] familyMembers = new ubkInputJsonDTO.FamilyMemberDTO[]
            { new() { pin = "123", lastname = "some", firstname = "some", patronymic = "some",
                roleId = 123 } };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            var fMemItem = familyMembers[0];
            ex.ParamName.Should().Be(nameof(fMemItem.role));
        }

        [Fact]
        public void WhenCalled_VerifyFamilyMembers_ThrowsFamilyMemberRoleIdNullError()
        {
            //Arrange
            ubkInputJsonDTO.FamilyMemberDTO[] familyMembers = new ubkInputJsonDTO.FamilyMemberDTO[]
            { new() { pin = "123", lastname = "some", firstname = "some", patronymic = "some",
                role = "some" } };
            IPersonalIdentityVerifier sut =
                new PersonalIdentityVerifierImpl(Mock.Of<IPassportDataVerifier>(),
                Mock.Of<IPersonDataVerifier>(), Mock.Of<IPinVerifier>());

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyFamilyMembers(familyMembers));

            //Assert
            var fMemItem = familyMembers[0];
            ex.ParamName.Should().Contain(nameof(fMemItem.roleId));
        }
    }
}
