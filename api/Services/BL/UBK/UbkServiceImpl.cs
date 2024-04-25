using api.Contracts.BL.CISSA;
using api.Contracts.BL.UBK;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Utils;

namespace api.Services.BL.UBK
{
    public class UbkServiceImpl : IUbkService
    {
        private readonly IUbkDataService _dataSvc;
        private readonly IUbkVerifier _ubkVerifier;
        private readonly IUbkInputDataParser _dataParser;
        private readonly ICissaDataProvider _cissaDataProvider;
        private readonly IHttpService _httpSvc;
        public UbkServiceImpl(IUbkDataService dataSvc, IUbkVerifier ubkVerifier, IUbkInputDataParser dataParser, ICissaDataProvider cissaDataProvider, IHttpService httpSvc)
        {
            _dataSvc = dataSvc;
            _ubkVerifier = ubkVerifier;
            _dataParser = dataParser;
            _cissaDataProvider = cissaDataProvider;
            _httpSvc = httpSvc;
        }

        public async Task<(string regNo, Guid appId)> CreateApplication(string json)
        {
            _ubkVerifier.VerifySrcJson(json);
            var parsedInputData = _dataParser.ParseFromJson(json);
            _ubkVerifier.VerifyParsedJsonData(parsedInputData);
            var pkgId = await _dataSvc.InsertSrcJsonToDb(json);
            (var regNo, var appId) = await _cissaDataProvider.CreateCissaApplication(parsedInputData.Applicant,
                StaticCissaReferences.PAYMENT_TYPE_UBK);
            await _dataSvc.UpdatePackageInfo(pkgId, regNo, appId);

            return (regNo, appId);
        }

        public async Task UpdatePackageInfo(setApplicationResultDTO? dto)
        {
            if(dto == null) throw new ArgumentNullException(nameof(dto), ErrorMessageResource.NullDataProvidedError);
            StaticReferences.CheckNulls(dto, "appId", "Decision");
            await _dataSvc.UpdatePackageInfo(appId: dto.appId!.Value, decision: dto.Decision!, dto.RejectionReason ?? "");
            var originAppId = await _dataSvc.GetOriginAppID(dto.appId!.Value);
            //TODO: call http microret's api
            await _httpSvc.CallChangeDecision(originAppId, dto.Decision!, dto.RejectionReason ?? "");
        }
    }
}
