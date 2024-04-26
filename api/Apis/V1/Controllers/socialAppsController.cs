using api.Contracts.BL;
using api.Models.BL;
using System.Text.Json;

namespace api.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/social-apps")]
    public class socialAppsController : ControllerBase
    {
        private readonly ISocialAppsService _socialAppsService;
        public socialAppsController(ISocialAppsService socialAppsService)
        {
            _socialAppsService = socialAppsService;
        }

        /// <summary>
        ///   Данная API принимает заявление на получение пособия Уй-булоого комок (UBK) и Ежемесячное социальное пособие (ESP)
        /// </summary>
        /// <response code="200">returns newly created ID</response>
        /// <response code="400">Error with description</response>
        [HttpPost("send-application/{paymentTypeCode}")]
        public async Task<createApplicationResultDTO> CreateApplication([FromBody]JsonElement data, string paymentTypeCode)
        {
            var (regNo, appId) = await _socialAppsService.CreateApplication(data.ToString(), paymentTypeCode);
            return new createApplicationResultDTO { regNo = regNo, appId = appId };
        }

        [HttpPost("set-result")]
        public async Task SetResult([FromBody] setApplicationResultDTO dto)
        {
            await _socialAppsService.SetApplicationResult(dto);
        }
    }
}
