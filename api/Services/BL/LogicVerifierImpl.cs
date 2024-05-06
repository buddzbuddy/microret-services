using api.Contracts.BL;
using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;
using api.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace api.Services.BL
{
    public class LogicVerifierImpl : ILogicVerifier
    {
        private readonly IPersonalIdentityVerifier _identityVerifier;
        private readonly IPropertyVerifier _propertyVerifier;

        public LogicVerifierImpl(IPersonalIdentityVerifier identityVerifier, IPropertyVerifier propertyVerifier)
        {
            _identityVerifier = identityVerifier;
            _propertyVerifier = propertyVerifier;
        }

        public void VerifyInputModel(InputModelDTO? inputModel, string paymentTypeCode)
        {
            if (inputModel == null)
                throw new ArgumentNullException(nameof(inputModel),
                    ErrorMessageResource.NullDataProvidedError);
            if (inputModel.ID == null)
                throw new ArgumentNullException(nameof(inputModel.ID),
                    ErrorMessageResource.NullDataProvidedError);
            _identityVerifier.VerifyApplicant(inputModel.Applicant);

            if (paymentTypeCode == StaticReferences.PAYMENT_TYPE_UBK)
            {
                _identityVerifier.VerifyFamilyMembers(inputModel.FamilyMembers);
                _propertyVerifier.VerifyProps(inputModel.Applicant);
            }
        }


    }
}
