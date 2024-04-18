using api.Models.BL;

namespace api.Contracts.BL.PropsValidations
{
    public interface ISFService
    {
        bool HasJobForPeriodOrLater(WorkPeriodInfoDTO? WorkPeriodInfo,
            int checkYear, int checkMonth);
    }
}
