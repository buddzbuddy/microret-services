namespace api.Contracts.BL.Verifiers
{
    public interface IPersonDataVerifier
    {
        void VerifyNames(string? surname, string? name, string? patronymic);
    }
}
