using static api.Models.BL.ubkInputJsonDTO;

namespace api.Contracts.BL.UBK
{
    public interface IPropertyVerifier
    {
        void VerifyParsedData(PersonDetailsInfo? personDetails);
    }
}
