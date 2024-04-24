using api.Contracts.BL.Verifiers;
using api.Contracts.BL.UBK;
using api.Models.BL;
using System.Linq;
using api.Utils;
using System.Text.RegularExpressions;

namespace api.Services.BL.Verifiers
{
    public class PassportDataVerifierImpl : IPassportDataVerifier
    {
        private readonly IPinVerifier _pinVerifier;
        public PassportDataVerifierImpl(IPinVerifier pinVerifier)
        {
            _pinVerifier = pinVerifier;
        }
        public void VerifyPassportData(PassportOnlyDTO? passport)
        {
            if (passport == null)
                throw new ArgumentNullException(nameof(passport),
                    ErrorMessageResource.NullDataProvidedError);
            _pinVerifier.VerifyPin(passport.Pin);
            StaticReferences.CheckNulls(passport, "PassportSeries", "PassportNumber",
                "PassportAuthority", "IssuedDate");

            //TODO: replace pattern to valid one
            /*var certNo = "";
            string pattern = @"^[a-zA-Z0-9]+\z";//checks letters and numbers ^[a-zA-Z0-9 ]+\z
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

            // Compare a string against the regular expression
            var validCertNo = System.Text.RegularExpressions.Regex.Replace(certNo, pattern, "");*/


            verifyPassportExpiration(passport);
        }

        
        private void verifyPassportExpiration(PassportOnlyDTO passport)
        {
            var today = DateTime.Today;
            if (passport.IssuedDate! > today) throw new ArgumentException(
                ErrorMessageResource.IllegalDataProvidedError,
                    nameof(passport.IssuedDate));
            var validTill =
                passport.IssuedDate!.Value.AddYears(StaticReferences.PASSPORT_DEFAULT_VALID_YEARS);
            if (passport.ExpiredDate != null) validTill = passport.ExpiredDate.Value;
            if (validTill <= today) throw new ArgumentException(
                ErrorMessageResource.PassportExpiredError, nameof(passport.IssuedDate));
        }
    }
}
