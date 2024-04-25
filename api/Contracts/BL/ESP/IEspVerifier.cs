using api.Models.BL;

namespace api.Contracts.BL.ESP
{
    public interface IEspVerifier
    {
        void VerifyInputModel(espInputModelDTO? inputModel);
    }
}
