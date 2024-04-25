using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IUbkService
    {
        Task<(string regNo, Guid appId)> CreateApplication(string json);
        Task UpdatePackageInfo(setApplicationResultDTO? dto);
    }
}
