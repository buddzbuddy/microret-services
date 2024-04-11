using api.Contracts.BL.PassportData;
using api.Contracts.BL.UBK;
using api.Models.BL;
using System.Linq;

namespace api.Services.BL.PassportData
{
    public class PassportDataVerifierImpl : IPassportDataVerifier
    {
        private readonly IPinVerifier _pinVerifier;
        public PassportDataVerifierImpl(IPinVerifier pinVerifier)
        {
            _pinVerifier = pinVerifier;
        }
        public void VerifyPassportData(PassportOnlyDTO passport)
        {
            _pinVerifier.VerifyPin(passport.Pin);
            if(string.IsNullOrEmpty(passport.PassportSeries)) 
                throw new ArgumentNullException(nameof(passport.PassportSeries),
                    ErrorMessageResource.NullDataProvidedError);
        }

        const int PASSPORT_DEFAULT_VALID_YEARS = 10;
        public void VerifyPassportExpiration(PassportOnlyDTO passport)
        {
            if (passport.IssuedDate == null)
                throw new ArgumentNullException(nameof(passport.IssuedDate),
                    ErrorMessageResource.NullDataProvidedError);
            var today = DateTime.Today;
            if (passport.IssuedDate.Value > today) throw new ArgumentException(
                ErrorMessageResource.IllegalDataProvidedError,
                    nameof(passport.IssuedDate));
            var validTill = passport.IssuedDate.Value.AddYears(PASSPORT_DEFAULT_VALID_YEARS);
            if (passport.ExpiredDate != null) validTill = passport.ExpiredDate.Value;
            if (validTill > today) throw new InvalidOperationException(
                ErrorMessageResource.PassportExpiredError);
        }
    }
}
