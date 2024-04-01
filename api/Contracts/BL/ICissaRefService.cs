using api.Models.BL;

namespace api.Contracts.BL
{
    public interface ICissaRefService
    {
        Task<double> GetGMI(gmiRequestDTO requestDTO);
    }
}
