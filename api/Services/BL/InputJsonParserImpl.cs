using api.Contracts.BL;
using api.Domain;
using api.Models.BL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.Services.BL
{
    public class InputJsonParserImpl : IInputJsonParser
    {
        public T? ParseToModel<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public void VerifyJson(string? json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    throw new DomainException(ErrorMessageResource.JsonEmptyError);
                var jObject = JObject.Parse(json);
                if (jObject == null || jObject.Count == 0)
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
    }
}
