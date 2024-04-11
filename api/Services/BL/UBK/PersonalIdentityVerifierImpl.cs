using api.Contracts.BL;
using api.Contracts.BL.PassportData;
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

            verifyPassportAndPersonInfo(applicant.PassportDataInfo);
        }


        const int ADULT_AGE_STARTS_FROM = 18;
        public void VerifyFamilyMembers(ubkInputJsonDTO.FamilyMemberDTO[]? familyMembers)
        {
            if (familyMembers == null || familyMembers.Length == 0)
                throw new ArgumentNullException(nameof(familyMembers),
                    ErrorMessageResource.NullDataProvidedError);

            foreach (var famlilyMember in familyMembers)
            {
                _pinVerifier.VerifyPin(famlilyMember.pin);
                _personDataVerifier.VerifyNames(
                    famlilyMember.lastname,
                    famlilyMember.firstname,
                    famlilyMember.patronymic);

                if(string.IsNullOrEmpty(famlilyMember.role))
                    throw new ArgumentNullException(nameof(famlilyMember.role),
                    ErrorMessageResource.NullDataProvidedError);
                if(famlilyMember.roleId == null || famlilyMember.roleId <= 0)
                    throw new ArgumentNullException(nameof(famlilyMember.roleId),
                    ErrorMessageResource.NullDataProvidedError);

                var birthDate = StaticReferences.ExtractBirthDate(famlilyMember.pin!);

                var age = StaticReferences.CalcAgeForToday(birthDate);
                if (age >= ADULT_AGE_STARTS_FROM)
                    verifyPassportAndPersonInfo(famlilyMember.PassportDataInfo);
                else
                    verifyBirthAct(famlilyMember.BirthActByPinInfo);

            }
        }


        private void verifyPassportAndPersonInfo(PassportDataInfoDTO? passportData)
        {
            if (passportData == null)
                throw new ArgumentNullException(nameof(passportData),
                    ErrorMessageResource.NullDataProvidedError);
            _passportDataVerifier.VerifyPassportData(passportData);
            _personDataVerifier.VerifyNames(passportData.Surname, passportData.Name,
                passportData.Patronymic);
            _passportDataVerifier.VerifyPassportExpiration(passportData);
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
    }
}
