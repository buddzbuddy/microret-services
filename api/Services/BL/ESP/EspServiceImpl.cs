using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.BL.ESP;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Utils;

namespace api.Services.BL.ESP
{
    public class EspServiceImpl : IEspService
    {
        private readonly IEspDataService _dataService;
        private readonly IInputJsonParser _dataParser;
        private readonly IEspVerifier _verifier;
        private readonly ICissaDataProvider _cissaDataProvider;
        private readonly IHttpService _httpSvc;
        public EspServiceImpl(IEspDataService dataService, IInputJsonParser dataParser, IEspVerifier verifier, ICissaDataProvider cissaDataProvider, IHttpService httpSvc)
        {
            _dataService = dataService;
            _dataParser = dataParser;
            _verifier = verifier;
            _cissaDataProvider = cissaDataProvider;
            _httpSvc = httpSvc;
        }

        public async Task<(string regNo, Guid appId)> CreateApplication(string json)
        {
            _dataParser.VerifyJson(json);
            var inputModel = _dataParser.ParseToModel<espInputModelDTO>(json);
            _verifier.VerifyInputModel(inputModel);
            var newPkgId = await _dataService.SaveJson(json);
            (var regNo, var appId) = await _cissaDataProvider.CreateCissaApplication(inputModel!.Applicant!);
            await _dataService.UpdatePackageInfo(newPkgId, regNo, appId);
            return (regNo, appId);
        }

        public async Task SetApplicationResult(setApplicationResultDTO? dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto), ErrorMessageResource.NullDataProvidedError);
            StaticReferences.CheckNulls(dto, "appId", "Decision");
            await _dataService.UpdatePackageInfo(appId: dto.appId!.Value, decision: dto.Decision!, dto.RejectionReason ?? "");
            var originAppId = await _dataService.GetOriginAppID(dto.appId!.Value);
            //Call http microret's api
            await _httpSvc.CallChangeDecision(originAppId, dto.Decision!, dto.RejectionReason ?? "");
        }
    }
}
