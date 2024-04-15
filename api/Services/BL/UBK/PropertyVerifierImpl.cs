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

        public void VerifyProps(ubkInputJsonDTO.PersonDetailsInfo? personDetails)
        {
            if(personDetails == null) throw new ArgumentNullException(nameof(personDetails),
                ErrorMessageResource.NullDataProvidedError);
            var age = _dataHelper.CalcAgeFromPinForToday(personDetails.pin!);
            if(age >= StaticReferences.ADULT_AGE_STARTS_FROM)
            {
                VerifyMarriageData(personDetails.MarriageActInfo);
                VerifyCars(personDetails.Cars);
                VerifyRealEstates(personDetails.RealEstateInfoList);
                VerifyUnemployeeStatus(personDetails.UnemployedStatusInfo);
                VerifyPension(personDetails.PensionInfo);
                VerifyMsec(personDetails.MSECDetailsInfo);
                VerifyAnimals(personDetails.AnimalDataList);
            }
        }
        
        public void VerifyCars(IEnumerable<CarDTO>? cars)
        {
            if (cars == null) return; //if no data provided, ignore it
            /*if (cars == null) throw new ArgumentNullException(nameof(cars),
                ErrorMessageResource.NullDataProvidedError);*/
            foreach (var car in cars)
            {
                StaticReferences.CheckNulls(car);
            }
        }
        public void VerifyRealEstates(IEnumerable<RealEstateInfoDTO>? props)
        {
            if (props == null) return;//if no data provided, ignore it
            /*if (props == null) throw new ArgumentNullException(nameof(props),
                ErrorMessageResource.NullDataProvidedError);*/
            foreach (var prop in props)
            {
                StaticReferences.CheckNullsWithExcludeProps(prop, "TermDate");
            }
        }
        public void VerifyMarriageData(MarriageActInfoDTO? marriageAct)
        {
            if (marriageAct == null) return;
            StaticReferences.CheckNulls(marriageAct);
            StaticReferences.CheckNulls(marriageAct.Crtf, "DocDate", "DocNumber", "DocSeries", "GovUnit");
            StaticReferences.CheckNulls(marriageAct.Groom, "Pin", "Surname", "FirstName");
            StaticReferences.CheckNulls(marriageAct.Bride, "Pin", "Surname", "FirstName");
        }
        public void VerifyUnemployeeStatus(UnemployedStatusInfoDTO? unemployedStatus)
        {
            if (unemployedStatus != null)
                StaticReferences.CheckNulls(unemployedStatus);
        }
        public void VerifyPension(PensionInfoDTO? pensionInfo)
        {
            if (pensionInfo != null)
                StaticReferences.CheckNullsWithExcludeProps(pensionInfo, "FullName");
        }
        public void VerifyMsec(MSECDetailsInfoDTO? data)
        {
            if (data == null) return;
            StaticReferences.CheckNullsWithExcludeProps(data,
                "ReExamination", "To", "InAbsentia", "IsDeathPeriod", "TimeOfDisability");
        }
        public void VerifyAnimals(AnimalDataDTO[]? animalDatas)
        {
            if(animalDatas == null) return;
            foreach (var item in animalDatas)
            {
                StaticReferences.CheckNulls(item);
            }
        }
    }
}
