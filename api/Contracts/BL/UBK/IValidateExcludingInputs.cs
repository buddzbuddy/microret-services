using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IValidateExcludingInputs
    {
        void Validate(PersonDetailsInfo? person);
    }
}
