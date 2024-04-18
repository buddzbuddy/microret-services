using api.Models.BL;

namespace api.Contracts.BL.PropsValidations
{
    public interface IUnemployeeValidator
    {
        bool IsUnemployee(UnemployedStatusInfoDTO? unemployedStatus);
    }
}
