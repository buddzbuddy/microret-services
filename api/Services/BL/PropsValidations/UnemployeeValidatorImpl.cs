using api.Contracts.BL.PropsValidations;
using api.Models.BL;
using api.Utils;
using System;

namespace api.Services.BL.PropsValidations
{
    public class UnemployeeValidatorImpl : IUnemployeeValidator
    {
        public bool IsUnemployee(UnemployedStatusInfoDTO? unemployedStatus)
        {
            return unemployedStatus?.Status == StaticReferences.UNEMPLOYEE_STATUS_NAME;
        }
    }
}
