using api.Contracts.BL.Verifiers;
using api.Utils;
using System.Linq;

namespace api.Services.BL.Verifiers
{
    public class PersonDataVerifierImpl : IPersonDataVerifier
    {
        
        public void VerifyNames(string? surname, string? name, string? patronymic)
        {
            verifyName(surname, nameof(surname));
            verifyName(name, nameof(name));
            if (surname!.ToUpper().EndsWith(StaticReferences.MALE_ENDS_WITH) ||
                surname!.ToUpper().EndsWith(StaticReferences.FEMALE_ENDS_WITH))
            {
                if (!string.IsNullOrEmpty(patronymic))
                    throw new ArgumentException(ErrorMessageResource.IllegalDataProvidedError,
                        nameof(patronymic));
            }
            else
            {
                verifyName(patronymic, nameof(patronymic));
            }
        }

        private void verifyName(string? src, string nameOfField)
        {
            if (string.IsNullOrEmpty(src))
                throw new ArgumentNullException(nameOfField, ErrorMessageResource.NullDataProvidedError);
            if (src.Length < StaticReferences.MINIMUM_NAME_LENGTH)
                throw new ArgumentException(
                    string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.MINIMUM_NAME_LENGTH), nameOfField);

            //TODO: Modify for using regex to allow -. and space
            if (src.Any(char.IsDigit))
                throw new ArgumentException(
                    ErrorMessageResource.StringShouldContainOnlyLettersError,
                    nameOfField);
        }
    }
}
