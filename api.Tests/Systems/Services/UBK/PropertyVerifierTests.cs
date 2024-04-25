using api.Contracts.BL.UBK;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Resources;
using api.Services.BL.UBK;
using api.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services.UBK
{
    public class PropertyVerifierTests : TestUtils
    {
        public PropertyVerifierTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void VerifyProps_WhenCalled_ThrowsNullError()
        {
            //Arrange
            PersonDetailsDTO? personDetails = null;
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyProps(personDetails));

            ex.ParamName.Should().Be(nameof(personDetails));
            ex.Message.Should().StartWith(ErrorMessageResource.NullDataProvidedError);
        }

        [Fact]
        public void VerifyCars_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            var cars = new CarDTO[] { new() };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyCars(cars));
            ex.ParamName.Should().Contain("Brand");
        }

        [Fact]
        public void VerifyCars_WhenCalled_Succeeded()
        {
            //Arrange
            var cars = new CarDTO[] { new()
            {
                BodyNo = "some", BodyType = "sedan",
                Brand = "some", CarTypeName = "some", Color = "some color", DateFrom = "some date",
                EngineVolume = 1, GovPlate = "some number", Model = "some model", Steering = "some",
                Vin = "some vin", Year = 1990 } };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyCars(cars);
        }

        [Fact]
        public void VerifyRealEstates_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            var props = new RealEstateInfoDTO[] { new() };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyRealEstates(props));
            ex.ParamName.Should().Contain("PropCode");
        }

        [Fact]
        public void VerifyRealEstates_WhenCalled_Succeeded()
        {
            //Arrange
            var props = new RealEstateInfoDTO[] { new()
            {
                Address = "some address", DocNum = "some number", Owner = "some owner", Pin = "123",
                PropCode = "some code", RegDate = new DateTime()
                } };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyRealEstates(props);
        }

        [Fact]
        public void VerifyMarriageData_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            MarriageActInfoDTO? marriageAct = new();
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyMarriageData(marriageAct));
            ex.ParamName.Should().Contain("Act");
        }

        [Fact]
        public void VerifyMarriageData_WhenCalled_Succeeded()
        {
            //Arrange
            MarriageActInfoDTO? marriageAct = new()
            {
                Act = "some number",
                Bride = new()
                {
                    Pin = "123",
                    Surname = "some surname",
                    FirstName = "some firstname"
                },
                Groom = new() { Pin = "123", Surname = "some surname", FirstName = "some firstname" },
                Crtf = new()
                {
                    DocDate = new DateTime(),
                    DocNumber = "123",
                    DocSeries = "some series",
                    GovUnit = "some unit name"
                }
            };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyMarriageData(marriageAct);
        }

        [Fact]
        public void VerifyUnemployeeStatus_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            UnemployedStatusInfoDTO? unemployedStatus = new();
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
            sut.VerifyUnemployeeStatus(unemployedStatus));
            ex.ParamName.Should().Contain("Status");
        }

        [Fact]
        public void VerifyUnemployeeStatus_WhenCalled_Succeeded()
        {
            //Arrange
            UnemployedStatusInfoDTO? unemployedStatus = new()
            {
                Status = "some status",
                EligibleForBenefits = false
            };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyUnemployeeStatus(unemployedStatus);
        }

        [Fact]
        public void VerifyPension_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            PensionInfoDTO? pensionInfo = new();
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
            sut.VerifyPension(pensionInfo));
            ex.ParamName.Should().Contain("Date");
        }

        [Fact]
        public void VerifyPension_WhenCalled_Succeeded()
        {
            //Arrange
            PensionInfoDTO? pensionInfo = new()
            {
                Date = new DateTime(),
                FirstName = "fn",
                LastName = "ln",
                Issuer = "iss",
                Pin = "123",
                State = "some state"
            };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyPension(pensionInfo);
        }

        [Fact]
        public void VerifyAnimals_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            var animalDatas = new AnimalDataDTO[] { new() };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.VerifyAnimals(animalDatas));
            ex.ParamName.Should().Contain("Age");
        }

        [Fact]
        public void VerifyAnimals_WhenCalled_Succeeded()
        {
            //Arrange
            AnimalDataDTO[]? animalDatas = new AnimalDataDTO[] { new()
            { Age = "5 mnth", Gender = "f", Type = "krs" } };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyAnimals(animalDatas);
        }

        [Fact]
        public void VerifyMsec_WhenCalled_ThrowsNullPropsError()
        {
            //Arrange
            MSECDetailsInfoDTO? data = new();
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
            sut.VerifyMsec(data));
            ex.ParamName.Should().Contain("DisabilityGroup");
        }

        [Fact]
        public void VerifyMsec_WhenCalled_Succeeded()
        {
            //Arrange
            MSECDetailsInfoDTO? data = new()
            {
                OrganizationName = "some org name",
                ExaminationDate = new DateTime(),
                ExaminationType = "primary",
                DisabilityGroup = "1st group",
                From = DateTime.Today,
                StatusCode = "some code"
            };
            IPropertyVerifier sut = new PropertyVerifierImpl(Mock.Of<IDataHelper>());

            //Act & Assert
            sut.VerifyMsec(data);
        }
    }
}
