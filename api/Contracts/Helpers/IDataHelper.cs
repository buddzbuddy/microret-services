namespace api.Contracts.Helpers
{
    public interface IDataHelper
    {
        DateTime ExtractBirthDate(string? pin);
        int CalcAgeFromPinForToday(string? pin);
        int CalcAgeForToday(DateTime? birthDate);
        DateTime GetDate(string? dateStr, string? format = "yyyy-MM-dd");
    }
}
