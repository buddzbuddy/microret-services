using api.Contracts.BL;
using api.Contracts.BL.CISSA;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Utils;

namespace api.Services.BL
{
    public class SocialAppsServiceImpl : ISocialAppsService
    {
        private readonly IDataService _dataSvc;
        private readonly ILogicVerifier _logicVerifier;
        private readonly IInputJsonParser _dataParser;
        private readonly ICissaDataProvider _cissaDataProvider;
        private readonly IHttpService _httpSvc;
        public SocialAppsServiceImpl(IDataService dataSvc, ILogicVerifier logicVerifier,
            IInputJsonParser dataParser, ICissaDataProvider cissaDataProvider, IHttpService httpSvc)
        {
            _dataSvc = dataSvc;
            _logicVerifier = logicVerifier;
            _dataParser = dataParser;
            _cissaDataProvider = cissaDataProvider;
            _httpSvc = httpSvc;
        }

        public async Task<(string regNo, Guid appId)> CreateApplication(string json, string paymentTypeCode)
        {
            Guid? paymenType = null;
            if(paymentTypeCode == StaticReferences.PAYMENT_TYPE_UBK)
            {
                paymenType = StaticCissaReferences.PAYMENT_TYPE_UBK;
            }
            else if (paymentTypeCode != StaticReferences.PAYMENT_TYPE_ESP)
            {
                throw new ArgumentException(ErrorMessageResource.IllegalDataProvidedError, nameof(paymentTypeCode));
            }
            _dataParser.VerifyJson(json);
            var parsedInputData = _dataParser.ParseToModel<ubkInputModelDTO>(json);
            _logicVerifier.VerifyInputModel(parsedInputData, paymentTypeCode);
            var newPkgId = await _dataSvc.SaveJson(json);
            (var regNo, var appId) =
                await _cissaDataProvider.CreateCissaApplication(parsedInputData!.Applicant!,
                paymenType);
            await _dataSvc.UpdatePackageInfo(newPkgId, regNo, appId);

            return (regNo, appId);
        }

        public async Task SetApplicationResult(setApplicationResultDTO? dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto), ErrorMessageResource.NullDataProvidedError);
            StaticReferences.CheckNulls(dto, "appId", "Decision");
            await _dataSvc.UpdatePackageInfo(appId: dto.appId!.Value, decision: dto.Decision!, dto.RejectionReason ?? "");
            var originAppId = await _dataSvc.GetOriginAppID(dto.appId!.Value);
            //Call http microret's api
            await _httpSvc.CallChangeDecision(originAppId, dto.Decision!, dto.RejectionReason ?? "");
        }
    }
}
