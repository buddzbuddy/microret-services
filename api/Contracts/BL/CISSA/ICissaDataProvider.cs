using api.Models.BL;
using static api.Models.BL.ubkInputModelDTO;

namespace api.Contracts.BL.CISSA
{
    public interface ICissaDataProvider
    {
        Task<(Guid orgId, Guid userId, Guid orgPositionId, string orgCode, long regNoLastValue)>
            DefineUserCredsFromAddressData(ResidentialAddressDTO? addressData);

        Task<(string regNo, Guid appId)>
            CreateCissaApplication(PersonDetailsDTO applicantPerson, Guid? paymentType = null);
    }
}
