namespace api.Contracts.BL.PassportData
{
    public interface IPinVerifier
    {
        void VerifyPin(string? pin);
    }
}
