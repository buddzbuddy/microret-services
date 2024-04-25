using api.Models.BL;

namespace api.Contracts.BL.ESP
{
    public interface IEspService
    {
        Task<(string regNo, Guid appId)> CreateApplication(string json);
        Task SetApplicationResult(setApplicationResultDTO? dto);
    }
}
