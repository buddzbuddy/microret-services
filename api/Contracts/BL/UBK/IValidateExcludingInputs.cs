using static api.Models.BL.ubkInputJsonDTO;

namespace api.Contracts.BL.UBK
{
    public interface IValidateExcludingInputs
    {
        void Validate(PersonDetailsInfo? person);
    }
}
