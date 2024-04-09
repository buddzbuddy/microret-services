using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IUbkVerifier
    {
        void VerifySrcJson(string? jsonData);
        void VerifyParsedJsonData(ubkInputJsonDTO? parsedDataJson);
    }
}
