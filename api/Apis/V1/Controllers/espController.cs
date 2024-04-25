using api.Contracts.BL.ESP;
using api.Models.BL;
using System.Text.Json;

namespace api.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class espController : ControllerBase
    {
        private readonly IEspService _espService;
        public espController(IEspService ubkService)
        {
            _espService = ubkService;
        }

        /// <summary>
        ///   Данная API принимает заявление на получение ежемесячного социального пособия
        /// </summary>
        /// <response code="200">returns newly created ID</response>
        /// <response code="400">Error with description</response>
        [HttpPost("create-application")]
        public async Task<createApplicationResultDTO> CreateApplication([FromBody]JsonElement data)
        {
            var (regNo, appId) = await _espService.CreateApplication(data.ToString());
            return new createApplicationResultDTO { regNo = regNo, appId = appId };
        }

        [HttpPost("set-result")]
        public async Task SetResult([FromBody] setApplicationResultDTO dto)
        {
            await _espService.SetApplicationResult(dto);
        }
    }
}
