namespace api.Contracts.BL.Verifiers
{
    public interface IPinVerifier
    {
        void VerifyPin(string? pin);
    }
}
