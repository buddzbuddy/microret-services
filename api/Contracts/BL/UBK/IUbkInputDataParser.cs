using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IUbkInputDataParser
    {
        ubkInputJsonDTO? ParseFromJson(string json);
    }
}
