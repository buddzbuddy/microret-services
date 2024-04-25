using api.Contracts.BL.UBK;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace api.Services.BL.UBK
{
    public class UbkVerifierImpl : IUbkVerifier
    {
        private readonly IPersonalIdentityVerifier _identityVerifier;
        private readonly IPropertyVerifier _propertyVerifier;

        public UbkVerifierImpl(IPersonalIdentityVerifier identityVerifier, IPropertyVerifier propertyVerifier)
        {
            _identityVerifier = identityVerifier;
            _propertyVerifier = propertyVerifier;
        }

        public void VerifyInputModel(ubkInputModelDTO? inputModel)
        {
            if (inputModel == null)
                throw new DomainException(ErrorMessageResource.JsonObjectNullError);
            if (inputModel.ID == null)
                throw new ArgumentNullException(nameof(inputModel.ID),
                    ErrorMessageResource.NullDataProvidedError);
            _identityVerifier.VerifyApplicant(inputModel.Applicant);
            _identityVerifier.VerifyFamilyMembers(inputModel.FamilyMembers);

            //TODO: Verify all props
            _propertyVerifier.VerifyProps(inputModel.Applicant);
        }

        
    }
}
