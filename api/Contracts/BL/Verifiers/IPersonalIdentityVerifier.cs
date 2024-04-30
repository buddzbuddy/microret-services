using api.Models.BL;

namespace api.Contracts.BL.Verifiers
{
    public interface IPersonalIdentityVerifier
    {
        void VerifyApplicant(PersonDetailsDTO? applicant);
        void VerifyFamilyMembers(InputModelDTO.FamilyMemberDTO[]? familyMembers);
    }
}
