using api.Contracts.BL.UBK;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Utils;

namespace api.Services.BL.UBK
{
    public class PropertyVerifierImpl : IPropertyVerifier
    {
        private readonly IDataHelper _dataHelper;

        public PropertyVerifierImpl(IDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        public void VerifyParsedData(ubkInputJsonDTO.PersonDetailsInfo? personDetails)
        {
            if(personDetails == null) throw new ArgumentNullException(nameof(personDetails),
                ErrorMessageResource.NullDataProvidedError);
            var age = _dataHelper.CalcAgeFromPinForToday(personDetails.pin!);
            if(age >= StaticReferences.ADULT_AGE_STARTS_FROM)
            {
                verifyMarriageData(personDetails.MarriageActInfo);
                verifyCars(personDetails.Cars);
                verifyRealEstates(personDetails.RealEstateInfoList);
                verifyUnemployeeStatus(personDetails.UnemployedStatusInfo);
                verifyPension(personDetails.PensionInfo);
                verifyMsec(personDetails.MSECDetailsInfo);
                verifyAnimals(personDetails.AnimalDataList);
            }
        }
        
        private void verifyCars(IEnumerable<CarDTO>? cars)
        {
            if (cars == null) return; //if no data provided, ignore it
            /*if (cars == null) throw new ArgumentNullException(nameof(cars),
                ErrorMessageResource.NullDataProvidedError);*/
            foreach (var car in cars)
            {
                StaticReferences.CheckNulls(car);
            }
        }
        private void verifyRealEstates(IEnumerable<RealEstateInfoDTO>? props)
        {
            if (props == null) return;//if no data provided, ignore it
            /*if (props == null) throw new ArgumentNullException(nameof(props),
                ErrorMessageResource.NullDataProvidedError);*/
            foreach (var prop in props)
            {
                StaticReferences.CheckNulls(prop, "TermDate");
            }
        }
        private void verifyMarriageData(MarriageActInfoDTO? marriageAct)
        {
            if (marriageAct == null) return;
            StaticReferences.CheckNulls(marriageAct);
            StaticReferences.CheckNulls(marriageAct.Crtf);
            StaticReferences.CheckNulls(marriageAct.Groom);
            StaticReferences.CheckNulls(marriageAct.Bride);
        }
        private void verifyUnemployeeStatus(UnemployedStatusInfoDTO? unemployedStatus)
        {
            if (unemployedStatus != null)
                StaticReferences.CheckNulls(unemployedStatus);
        }
        private void verifyPension(PensionInfoDTO? pensionInfo)
        {
            if (pensionInfo == null) StaticReferences.CheckNulls(pensionInfo, "FullName");
        }
        private void verifyMsec(MSECDetailsInfoDTO? data)
        {
            if (data == null) return;
            StaticReferences.CheckNulls(data,
                "ReExamination", "InAbsentia", "IsDeathPeriod", "TimeOfDisability");
        }
        private void verifyAnimals(AnimalDataDTO[]? animalDatas)
        {
            if(animalDatas == null) return;
            foreach (var item in animalDatas)
            {
                StaticReferences.CheckNulls(item);
            }
        }
    }
}
