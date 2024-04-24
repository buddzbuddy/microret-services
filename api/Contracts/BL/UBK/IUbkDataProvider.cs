using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IUbkDataProvider
    {
        Task SendToCissa(ubkInputJsonDTO? data);
    }
}
