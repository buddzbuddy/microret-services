using System.Globalization;

namespace api.Utils
{
    public static class StaticReferences
    {
        const string PIN_BIRTHDATE_SECTION_FORMAT = "ddMMyyyy";
        public static DateTime ExtractBirthDate(string pin)
        {
            if(pin.Length != 14) throw new ArgumentException(string.Format(
                ErrorMessageResource.InvalidStringLengthError, 14), nameof(pin));
            var birthDateStr = pin.Substring(1, 8);
            if (DateTime.TryParseExact(birthDateStr, PIN_BIRTHDATE_SECTION_FORMAT,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate))
                return birthDate;
            else throw new ArgumentException(
                ErrorMessageResource.PinBirthDateSectionIllegalError, nameof(pin));
        }

        public static int CalcAgeForToday(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
