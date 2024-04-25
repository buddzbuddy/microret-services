using api.Models.BL;

namespace api.Contracts.BL.UBK
{
    public interface IUbkVerifier
    {
        void VerifyInputModel(ubkInputModelDTO? inputModel);
    }
}
