using api.Contracts.BL.PassportData;
using System.Linq;

namespace api.Services.BL.PassportData
{
    public class PinVerifierImpl : IPinVerifier
    {
        const int PIN_LENGTH = 14;
        public void VerifyPin(string? pin)
        {
            if (string.IsNullOrEmpty(pin))
                throw new ArgumentNullException(nameof(pin));
            if (pin.Length != PIN_LENGTH)
                throw new ArgumentException(string.Format(ErrorMessageResource.InvalidStringLengthError,
                    PIN_LENGTH), nameof(pin));
            if (!pin.All(char.IsDigit))
                throw new ArgumentException(ErrorMessageResource.StringShouldContainDigitsError,
                    nameof(pin));
        }
    }
}
