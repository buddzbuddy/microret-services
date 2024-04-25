using api.Contracts.BL.ESP;
using api.Contracts.BL.Verifiers;
using api.Domain;
using api.Models.BL;

namespace api.Services.BL.ESP
{
    public class EspVerifierImpl : IEspVerifier
    {
        private readonly IPersonalIdentityVerifier _identityVerifier;

        public EspVerifierImpl(IPersonalIdentityVerifier identityVerifier)
        {
            _identityVerifier = identityVerifier;
        }

        public void VerifyInputModel(espInputModelDTO? inputModel)
        {
            if (inputModel == null)
                throw new DomainException(ErrorMessageResource.JsonObjectNullError);
            if (inputModel.ID == null)
                throw new ArgumentNullException(nameof(inputModel.ID),
                    ErrorMessageResource.NullDataProvidedError);
            _identityVerifier.VerifyApplicant(inputModel.Applicant);
        }
    }
}
