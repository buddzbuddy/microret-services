namespace api.Contracts.BL.PassportData
{
    public interface IPersonDataVerifier
    {
        void VerifyNames(string? surname, string? name, string? patronymic);
    }
}
