using api.Contracts.BL.CISSA;
using api.Contracts.BL.UBK;
using api.Utils;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace api.Services.BL.UBK
{
    public class UbkServiceImpl : IUbkService
    {
        private readonly IUbkDataService _dataSvc;
        private readonly IUbkVerifier _ubkVerifier;
        private readonly IUbkInputDataParser _dataParser;
        private readonly ICissaDataProvider _cissaDataProvider;
        public UbkServiceImpl(IUbkDataService dataSvc, IUbkVerifier ubkVerifier, IUbkInputDataParser dataParser, ICissaDataProvider cissaDataProvider)
        {
            _dataSvc = dataSvc;
            _ubkVerifier = ubkVerifier;
            _dataParser = dataParser;
            _cissaDataProvider = cissaDataProvider;
        }

        public async Task<(string regNo, Guid appId)> CreateApplication(string json)
        {
            _ubkVerifier.VerifySrcJson(json);
            var parsedInputData = _dataParser.ParseFromJson(json);
            _ubkVerifier.VerifyParsedJsonData(parsedInputData);
            await _dataSvc.InsertSrcJsonToDb(json);
            (var regNo, var appId) = await _cissaDataProvider.CreateCissaApplication(parsedInputData.Applicant,
                StaticCissaReferences.PAYMENT_TYPE_UBK);

            return (regNo, appId);
        }
    }
}
