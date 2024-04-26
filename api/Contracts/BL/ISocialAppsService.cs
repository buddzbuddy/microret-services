using api.Models.BL;

namespace api.Contracts.BL
{
    public interface ISocialAppsService
    {
        Task<(string regNo, Guid appId)> CreateApplication(string json, string paymentTypeCode);
        Task SetApplicationResult(setApplicationResultDTO? dto);
    }
}
