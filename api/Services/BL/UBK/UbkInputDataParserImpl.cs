using api.Contracts.BL.UBK;
using api.Models.BL;
using Newtonsoft.Json;

namespace api.Services.BL.UBK
{
    public class UbkInputDataParserImpl : IUbkInputDataParser
    {
        public ubkInputJsonDTO? ParseFromJson(string json)
        {
            return JsonConvert.DeserializeObject<ubkInputJsonDTO>(json);
        }
    }
}
