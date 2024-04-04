using api.Contracts.BL.UBK;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace api.Services.BL.UBK
{
    public class UbkServiceImpl : IUbkService
    {
        private readonly IUbkDataService _dataSvc;
        public UbkServiceImpl(IUbkDataService dataSvc)
        {
            _dataSvc = dataSvc;
        }

        public async Task<int> CreateApplication(string json)
        {
            return await _dataSvc.InsertSrcJsonToDb(json);
        }
    }
}
