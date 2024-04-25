using api.Contracts.BL;
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
        private readonly IInputJsonParser _dataParser;
        private readonly ICissaDataProvider _cissaDataProvider;
        private readonly IHttpService _httpSvc;
        public UbkServiceImpl(IUbkDataService dataSvc, IUbkVerifier ubkVerifier, IInputJsonParser dataParser, ICissaDataProvider cissaDataProvider, IHttpService httpSvc)
        {
            _dataSvc = dataSvc;
            _ubkVerifier = ubkVerifier;
            _dataParser = dataParser;
            _cissaDataProvider = cissaDataProvider;
            _httpSvc = httpSvc;
        }

        public async Task<(string regNo, Guid appId)> CreateApplication(string json)
        {
            _dataParser.VerifyJson(json);
            var parsedInputData = _dataParser.ParseToModel<ubkInputModelDTO>(json);
            _ubkVerifier.VerifyInputModel(parsedInputData);
            var newPkgId = await _dataSvc.SaveJson(json);
            (var regNo, var appId) = await _cissaDataProvider.CreateCissaApplication(parsedInputData!.Applicant!,
                StaticCissaReferences.PAYMENT_TYPE_UBK);
            await _dataSvc.UpdatePackageInfo(newPkgId, regNo, appId);

            return (regNo, appId);
        }

        public async Task SetApplicationResult(setApplicationResultDTO? dto)
        {
            if(dto == null) throw new ArgumentNullException(nameof(dto), ErrorMessageResource.NullDataProvidedError);
            StaticReferences.CheckNulls(dto, "appId", "Decision");
            await _dataSvc.UpdatePackageInfo(appId: dto.appId!.Value, decision: dto.Decision!, dto.RejectionReason ?? "");
            var originAppId = await _dataSvc.GetOriginAppID(dto.appId!.Value);
            //Call http microret's api
            await _httpSvc.CallChangeDecision(originAppId, dto.Decision!, dto.RejectionReason ?? "");
        }
    }
}
