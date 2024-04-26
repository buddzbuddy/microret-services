using api.Models.BL;

namespace api.Contracts.BL
{
    public interface ILogicVerifier
    {
        void VerifyInputModel(ubkInputModelDTO? inputModel, string paymentTypeCode);
    }
}
