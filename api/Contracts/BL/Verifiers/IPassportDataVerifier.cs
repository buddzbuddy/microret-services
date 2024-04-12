using api.Models.BL;

namespace api.Contracts.BL.Verifiers
{
    public interface IPassportDataVerifier
    {
        void VerifyPassportData(PassportOnlyDTO? passport);
    }
}
