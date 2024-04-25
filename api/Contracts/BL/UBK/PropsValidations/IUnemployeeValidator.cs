using api.Models.BL;

namespace api.Contracts.BL.UBK.PropsValidations
{
    public interface IUnemployeeValidator
    {
        bool IsUnemployee(UnemployedStatusInfoDTO? unemployedStatus);
    }
}
