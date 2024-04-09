using api.Contracts.BL.UBK;
using api.Domain;
using api.Models.BL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace api.Services.BL.UBK
{
    public class UbkVerifierImpl : IUbkVerifier
    {
        public void VerifySrcJson(string? jsonData)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonData)) throw new DomainException(ErrorMessageResource.JsonEmptyError);
                var jObject = JObject.Parse(jsonData);
                if(jObject == null || jObject.Count == 0)
                {
                    throw new DomainException(ErrorMessageResource.JsonEmptyError);
                }
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new DomainException(ErrorMessageResource.JsonInvalidError);
            }
            
        }

        public void VerifyParsedJsonData(ubkInputJsonDTO? parsedDataJson)
        {
            if (parsedDataJson == null)
                throw new DomainException(ErrorMessageResource.JsonObjectNullError);

        }

    }
}
