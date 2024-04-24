using api.Contracts.BL.UBK;
using api.Models.BL;
using System.Text.Json;

namespace api.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ubkController : ControllerBase
    {
        private readonly IUbkService ubkService;
        public ubkController(IUbkService ubkService)
        {
            this.ubkService = ubkService;
        }

        /// <summary>
        ///   Данная API принимает заявление на получение пособия Уй-булоого комок
        /// </summary>
        /// <response code="200">returns newly created ID</response>
        /// <response code="400">Error with description</response>
        [HttpPost("create-application")]
        public async Task<createApplicationResultDTO> CreateApplication([FromBody]JsonElement data)
        {
            var result = await ubkService.CreateApplication(data.ToString());
            return new createApplicationResultDTO { regNo = result.regNo, appId = result.appId };
        }

        [HttpGet("get-hello")]
        public string getHello() => "Hello New Fourth change";
    }
}
