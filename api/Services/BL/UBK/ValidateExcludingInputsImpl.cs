using api.Contracts.BL.PropsValidations;
using api.Contracts.BL.UBK;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Models.Enums;
using api.Utils;

namespace api.Services.BL.UBK
{
    public class ValidateExcludingInputsImpl : IValidateExcludingInputs
    {
        private readonly IDataHelper _dataHelper;
        private readonly IUnemployeeValidator _unemployeeValidator;

        public ValidateExcludingInputsImpl(IDataHelper dataHelper, IUnemployeeValidator unemployeeValidator)
        {
            _dataHelper = dataHelper;
            _unemployeeValidator = unemployeeValidator;
        }

        public void Validate(ubkInputJsonDTO.PersonDetailsInfo? person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person), ErrorMessageResource.NullDataProvidedError);

            var age = _dataHelper.CalcAgeFromPinForToday(person.pin);
            var genderType = _dataHelper.GetGender(person.pin);
            
            if (genderType == GenderType.MALE && age < StaticReferences.MEN_RETIREMENT_AGE)
            {//Positive Unemployee Status concerns

                if (person.UnemployedStatusInfo == null)
                    throw new ArgumentNullException(nameof(person.UnemployedStatusInfo), ErrorMessageResource.NullDataProvidedError);
                
                if(!_unemployeeValidator.IsUnemployee(person.UnemployedStatusInfo))
                    throw new ArgumentException(ErrorMessageResource.UnemployeeStatusIncorrectError, nameof(person.UnemployedStatusInfo.Status));
            }
        }
    }
}
