using api.Contracts.BL.UBK;
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
        ///   This API creates and returns newly created ID of application
        /// </summary>
        /// <response code="200">returns newly created ID</response>
        /// <response code="400">Error with description</response>
        [HttpPost("create-application")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<int> CreateApplication([FromBody]JsonElement data)
        {
            var result = await ubkService.CreateApplication(data.ToString());
            return result;
        }

        [HttpGet("get-hello")]
        public string getHello() => "Hello New Second change";
    }
}
