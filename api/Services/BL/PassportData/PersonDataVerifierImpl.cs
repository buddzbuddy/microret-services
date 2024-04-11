using api.Contracts.BL.PassportData;
using System.Linq;

namespace api.Services.BL.PassportData
{
    public class PersonDataVerifierImpl : IPersonDataVerifier
    {
        const int MINIMUM_NAME_LENGTH = 2;
        const string MALE_ENDS_WITH = "УУЛУ";
        const string FEMALE_ENDS_WITH = "КЫЗЫ";
        public void VerifyNames(string? surname, string? name, string? patronymic)
        {
            verifyLength(surname, nameof(surname));
            verifyLength(name, nameof(name));
            if (surname!.ToUpper().EndsWith(MALE_ENDS_WITH) ||
                surname!.ToUpper().EndsWith(FEMALE_ENDS_WITH))
                if (!string.IsNullOrEmpty(patronymic))
                    throw new ArgumentException(ErrorMessageResource.IllegalDataProvidedError,
                        nameof(patronymic));
        }

        private void verifyLength(string? src, string nameOfField)
        {
            if (string.IsNullOrEmpty(src))
                throw new ArgumentNullException(nameOfField);
            if (src.Length != MINIMUM_NAME_LENGTH)
                throw new ArgumentException(
                    string.Format(ErrorMessageResource.InvalidStringLengthError,
                    MINIMUM_NAME_LENGTH), nameOfField);
            if (!src.All(char.IsDigit))
                throw new ArgumentException(
                    ErrorMessageResource.StringShouldNotContainDigitsError,
                    nameOfField);
        }
    }
}
