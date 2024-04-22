using api.Contracts.BL.Verifiers;
using api.Models.Enums;
using api.Utils;
using System.Linq;

namespace api.Services.BL.Verifiers
{
    public class PinVerifierImpl : IPinVerifier
    {
        public void VerifyPin(string? pin)
        {
            if (string.IsNullOrEmpty(pin))
                throw new ArgumentNullException(nameof(pin), ErrorMessageResource.NullDataProvidedError);
            if (pin.Length != StaticReferences.PIN_LENGTH)
                throw new ArgumentException(string.Format(ErrorMessageResource.InvalidStringLengthError,
                    StaticReferences.PIN_LENGTH), nameof(pin));
            if (!pin.All(char.IsDigit))
                throw new ArgumentException(ErrorMessageResource.StringShouldContainOnlyDigitsError,
                    nameof(pin));
            var genderType = int.Parse(pin[0].ToString());
            var existingGenders = new[] { (int)GenderType.MALE, (int)GenderType.FEMALE };
            if(!existingGenders.Contains(genderType))
                throw new ArgumentException($"{ErrorMessageResource.IllegalDataProvidedError} - src: {genderType}", nameof(pin));
        }
    }
}
