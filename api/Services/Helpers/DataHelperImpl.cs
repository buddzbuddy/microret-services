using api.Contracts.BL.Verifiers;
using api.Contracts.Helpers;
using api.Models.Enums;
using api.Utils;
using System.Globalization;

namespace api.Services.Helpers
{
    public class DataHelperImpl : IDataHelper
    {
        private readonly IPinVerifier _pinVerifier;
        public DataHelperImpl(IPinVerifier pinVerifier)
        {
            _pinVerifier = pinVerifier;
        }
        public int CalcAgeForToday(DateTime? birthDate)
        {
            if(birthDate == null) throw new ArgumentNullException(nameof(birthDate),
                ErrorMessageResource.NullDataProvidedError);
            var today = DateTime.Today;
            if(birthDate > today) throw new ArgumentException(
                ErrorMessageResource.IllegalDataProvidedError, nameof(birthDate));
            var age = today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > today.AddYears(-age)) age--;
            return age;
        }

        public int CalcAgeFromPinForToday(string? pin) => CalcAgeForToday(ExtractBirthDate(pin));

        public DateTime ExtractBirthDate(string? pin)
        {
            _pinVerifier.VerifyPin(pin);
            var birthDateStr = pin!.Substring(1, 8);
            return GetDate(birthDateStr, StaticReferences.PIN_BIRTHDATE_SECTION_FORMAT);
        }

        public DateTime GetDate(string? dateStr, string? format = "yyyy-MM-dd")
        {
            if (string.IsNullOrEmpty(dateStr))
                throw new ArgumentNullException(nameof(dateStr),
                    ErrorMessageResource.NullDataProvidedError);
            if (DateTime.TryParseExact(dateStr, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
                return date;
            throw new FormatException($"Couldn't be parsed given {nameof(dateStr)}" +
                $" to DateTime by format: {format}. Given src: {dateStr}");
        }

        public GenderType GetGender(string? pin)
        {
            _pinVerifier.VerifyPin(pin);
            return (GenderType)int.Parse(pin![0].ToString());
        }
    }
}
