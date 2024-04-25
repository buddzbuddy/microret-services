using api.Models.BL;

namespace api.Contracts.BL.CISSA
{
    public interface ICissaRefService
    {
        Task<double> GetGMI(gmiRequestDTO requestDTO);
    }
}
