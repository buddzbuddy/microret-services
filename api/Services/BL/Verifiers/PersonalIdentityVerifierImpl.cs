using api.Contracts.BL;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;
using api.Utils;
using System.Linq;
using api.Contracts.Helpers;

namespace api.Services.BL.Verifiers
{
    public class PersonalIdentityVerifierImpl : IPersonalIdentityVerifier
    {
        private readonly IPassportDataVerifier _passportDataVerifier;
        private readonly IPersonDataVerifier _personDataVerifier;
        private readonly IPinVerifier _pinVerifier;
        private readonly IDataHelper _dataHelper;
        public PersonalIdentityVerifierImpl(IPassportDataVerifier passportDataVerifier,
            IPersonDataVerifier personDataVerifier, IPinVerifier pinVerifier,
            IDataHelper dataHelper)
        {
            _passportDataVerifier = passportDataVerifier;
            _personDataVerifier = personDataVerifier;
            _pinVerifier = pinVerifier;
            _dataHelper = dataHelper;
        }
        public void VerifyApplicant(PersonDetailsDTO? applicant)
        {
            if (applicant == null) throw new ArgumentNullException(nameof(applicant),
                ErrorMessageResource.NullDataProvidedError);
            _pinVerifier.VerifyPin(applicant.pin);

            verifyPassportAndPersonInfo(applicant.PassportDataInfo);
            verifyFactAddress(applicant.ResidentialAddress);
        }
        public void VerifyFamilyMembers(InputModelDTO.FamilyMemberDTO[]? familyMembers)
        {
            if (familyMembers == null || familyMembers.Length == 0)
                throw new ArgumentNullException(nameof(familyMembers),
                    ErrorMessageResource.NullDataProvidedError);

            foreach (var famlilyMember in familyMembers)
            {
                StaticReferences.CheckNulls(famlilyMember, "pin", "lastname", "firstname",
                    "role", "roleId");

                _pinVerifier.VerifyPin(famlilyMember.pin);
                _personDataVerifier.VerifyNames(
                    famlilyMember.lastname,
                    famlilyMember.firstname,
                    famlilyMember.patronymic);

                var age = _dataHelper.CalcAgeFromPinForToday(famlilyMember.pin);
                if (age >= StaticReferences.ADULT_AGE_STARTS_FROM)
                    verifyPassportAndPersonInfo(famlilyMember.PassportDataInfo);
                else
                    verifyBirthAct(famlilyMember.BirthActByPinInfo);

            }
        }
        private void verifyPassportAndPersonInfo(PassportDataInfoDTO? passportData)
        {
            _passportDataVerifier.VerifyPassportData(passportData);
            _personDataVerifier.VerifyNames(passportData?.Surname, passportData?.Name,
                passportData?.Patronymic);
        }
        private void verifyBirthAct(BirthActByPinInfoDTO? birthAct)
        {
            if (birthAct == null)
                throw new ArgumentNullException(nameof(birthAct),
                    ErrorMessageResource.NullDataProvidedError);

            StaticReferences.CheckNulls(birthAct, "ActDate", "ActNumber", "ActGovUnit", "ChildSurname",
                "ChildFirstName", "ChildGender", "ChildPlaceOfBirth", "MotherPin", "MotherSurname",
                "MotherFirstName", "MotherNationality", "MotherCitizenship");

            _personDataVerifier.VerifyNames(
                    birthAct.ChildSurname,
                    birthAct.ChildFirstName,
                    birthAct.ChildPatronymic);

            _pinVerifier.VerifyPin(birthAct.MotherPin);
            _personDataVerifier.VerifyNames(
                    birthAct.MotherSurname,
                    birthAct.MotherFirstName,
                    birthAct.MotherPatronymic);

            if (!string.IsNullOrEmpty(birthAct.FatherPin))
            {
                StaticReferences.CheckNulls(birthAct, "FatherSurname", "FatherFirstName",
                    "FatherNationality", "FatherCitizenship");
                _pinVerifier.VerifyPin(birthAct.FatherPin);
                _personDataVerifier.VerifyNames(
                        birthAct.FatherSurname,
                        birthAct.FatherFirstName,
                        birthAct.FatherPatronymic);
            }

        }
        private void verifyFactAddress(ResidentialAddressDTO? address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address),
                    ErrorMessageResource.NullDataProvidedError);
            StaticReferences.CheckNulls(address, "State", "StateId", "StateCode", "Region", "RegionId",
                "RegionCode", "Street", "StreetId", "StreetCode", "House");
        }
    }
}
