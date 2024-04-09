using api.Contracts.BL.UBK;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace api.Services.BL.UBK
{
    public class UbkServiceImpl : IUbkService
    {
        private readonly IUbkDataService _dataSvc;
        private readonly IUbkVerifier _ubkVerifier;
        private readonly IUbkInputDataParser _dataParser;
        public UbkServiceImpl(IUbkDataService dataSvc, IUbkVerifier ubkVerifier, IUbkInputDataParser dataParser)
        {
            _dataSvc = dataSvc;
            _ubkVerifier = ubkVerifier;
            _dataParser = dataParser;
        }

        public async Task<int> CreateApplication(string json)
        {
            _ubkVerifier.VerifySrcJson(json);
            var parsedInputData = _dataParser.ParseFromJson(json);
            _ubkVerifier.VerifyParsedJsonData(parsedInputData);

            return await _dataSvc.InsertSrcJsonToDb(json);
        }
    }
}
