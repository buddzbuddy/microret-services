using api.Models.BL;

namespace api.Contracts.BL.PassportData
{
    public interface IPassportDataVerifier
    {
        void VerifyPassportData(PassportOnlyDTO passport);
        void VerifyPassportExpiration(PassportOnlyDTO passport);
    }
}
