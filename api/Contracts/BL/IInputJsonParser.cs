using api.Models.BL;

namespace api.Contracts.BL
{
    public interface IInputJsonParser
    {
        T? ParseToModel<T>(string json);
        void VerifyJson(string? json);
    }
}
