﻿using api.Models.BL;
using static api.Models.BL.ubkInputJsonDTO;

namespace api.Contracts.BL.UBK
{
    public interface IPropertyVerifier
    {
        void VerifyProps(PersonDetailsInfo? personDetails);
        void VerifyCars(IEnumerable<CarDTO>? cars);
        void VerifyRealEstates(IEnumerable<RealEstateInfoDTO>? props);
        void VerifyMarriageData(MarriageActInfoDTO? marriageAct);
        void VerifyUnemployeeStatus(UnemployedStatusInfoDTO? unemployedStatus);
        void VerifyPension(PensionInfoDTO? pensionInfo);
        void VerifyMsec(MSECDetailsInfoDTO? data);
        void VerifyAnimals(AnimalDataDTO[]? animalDatas);
    }
}
