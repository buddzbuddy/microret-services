using api.Models.BL;

namespace api.Contracts.BL.UBK.PropsValidations
{
    public interface ISFService
    {
        bool HasJobForPeriodOrLater(WorkPeriodInfoDTO? WorkPeriodInfo,
            int checkYear, int checkMonth);
    }
}
