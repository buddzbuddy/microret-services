using api.Contracts.BL.UBK;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace api.Services.BL.UBK
{
    public class UbkServiceImpl : IUbkService
    {
        private readonly IUbkDataService _dataSvc;
        private readonly IUbkVerifier _ubkVerifier;
        public UbkServiceImpl(IUbkDataService dataSvc, IUbkVerifier ubkVerifier)
        {
            _dataSvc = dataSvc;
            _ubkVerifier = ubkVerifier;
        }

        public async Task<int> CreateApplication(string json)
        {
            _ubkVerifier.VerifySrcJson(json);
            return await _dataSvc.InsertSrcJsonToDb(json);
        }
    }
}
