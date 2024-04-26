using api.Contracts.BL.CISSA;
using api.Models.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CissaRefsController : ControllerBase
    {
        private readonly ICissaRefService cissaRefSvc;
        public CissaRefsController(ICissaRefService cissaRefService)
        {
            cissaRefSvc = cissaRefService;
        }

        [HttpGet("get-gmi/{year}/{month}")]
        public async Task<IActionResult> GetGMI(int year, int month)
        {
            var requestModel = new gmiRequestDTO { year = year, month = month };
            return Ok(await cissaRefSvc.GetGMI(requestModel));
        }
    }
}
