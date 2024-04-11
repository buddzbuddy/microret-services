using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IPersonalIdentityVerifier
    {
        void VerifyApplicant(ubkInputJsonDTO.ApplicantDTO? applicant);
        void VerifyFamilyMembers(ubkInputJsonDTO.FamilyMemberDTO[]? familyMembers);
    }
}
