using api.Contracts.BL;
using api.Contracts.BL.Verifiers;
using api.Contracts.BL.UBK;
using api.Domain;
using api.Models.BL;
using api.Utils;
using System.Linq;

namespace api.Services.BL.UBK
{
    public class PersonalIdentityVerifierImpl : IPersonalIdentityVerifier
    {
        private readonly IPassportDataVerifier _passportDataVerifier;
        private readonly IPersonDataVerifier _personDataVerifier;
        private readonly IPinVerifier _pinVerifier;
        public PersonalIdentityVerifierImpl(IPassportDataVerifier passportDataVerifier,
            IPersonDataVerifier personDataVerifier, IPinVerifier pinVerifier)
        {
            _passportDataVerifier = passportDataVerifier;
            _personDataVerifier = personDataVerifier;
            _pinVerifier = pinVerifier;
        }
        public void VerifyApplicant(ubkInputJsonDTO.ApplicantDTO? applicant)
        {
            if (applicant == null) throw new ArgumentNullException(nameof(applicant),
                ErrorMessageResource.NullDataProvidedError);
            _pinVerifier.VerifyPin(applicant.pin);

            verifyPassportAndPersonInfo(applicant.PassportDataInfo);
            verifyFactAddress(applicant.ResidentialAddress);
        }
        public void VerifyFamilyMembers(ubkInputJsonDTO.FamilyMemberDTO[]? familyMembers)
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

                var age = StaticReferences.CalcAgeFromPinForToday(famlilyMember.pin);
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

            if(birthAct.ActDate == null)
                throw new ArgumentNullException(nameof(birthAct.ActDate),
                    ErrorMessageResource.NullDataProvidedError);
            if (string.IsNullOrEmpty(birthAct.ActNumber))
                throw new ArgumentNullException(nameof(birthAct.ActNumber),
                    ErrorMessageResource.NullDataProvidedError);
            if (string.IsNullOrEmpty(birthAct.ActGovUnit))
                throw new ArgumentNullException(nameof(birthAct.ActGovUnit),
                    ErrorMessageResource.NullDataProvidedError);

            _personDataVerifier.VerifyNames(
                    birthAct.ChildSurname,
                    birthAct.ChildFirstName,
                    birthAct.ChildPatronymic);

            if (birthAct.ChildGender == null)
                throw new ArgumentNullException(nameof(birthAct.ChildGender),
                    ErrorMessageResource.NullDataProvidedError);
            if (string.IsNullOrEmpty(birthAct.ChildPlaceOfBirth))
                throw new ArgumentNullException(nameof(birthAct.ChildPlaceOfBirth),
                    ErrorMessageResource.NullDataProvidedError);

            _pinVerifier.VerifyPin(birthAct.MotherPin);
            _personDataVerifier.VerifyNames(
                    birthAct.MotherSurname,
                    birthAct.MotherFirstName,
                    birthAct.MotherPatronymic);

            //TODO: MotherNationality, MotherCitizenship check later

            //TODO: Father's data verify by need

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
