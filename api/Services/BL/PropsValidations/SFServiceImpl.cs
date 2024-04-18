using api.Contracts.BL.PropsValidations;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Utils;
using System.Linq;

namespace api.Services.BL.PropsValidations
{
    public class SFServiceImpl : ISFService
    {
        private readonly IDataHelper _dataHelper;

        public SFServiceImpl(IDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public bool HasJobForPeriodOrLater(WorkPeriodInfoDTO? WorkPeriodInfo,
            int checkYear, int checkMonth)
        {
            if(WorkPeriodInfo != null)
            {
                if(WorkPeriodInfo.WorkPeriods != null && WorkPeriodInfo.WorkPeriods.Length > 0)
                {
                    var lastJobDate = WorkPeriodInfo.WorkPeriods
                        .Max(x => _dataHelper.GetDate(x.DateEnd, StaticReferences.WORK_PERIOD_DATE_FORMAT));

                    return (lastJobDate.Year * 100 + lastJobDate.Month) >= (checkYear * 100 + checkMonth);
                }
            }
            return false;
            
        }
    }
}
