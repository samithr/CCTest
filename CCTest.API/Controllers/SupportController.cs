using CCTest.API.ApiResponses;
using CCTest.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CCTest.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        public async Task<IActionResult> InitiateAsync(string userId)
        {
            var response = await _supportService.InitiateSupportRequest(userId);
            if (!response.IsError)
            {
                return Ok(new OkResponse(response.Message, response.Result));
            }
            else
            {
                return BadRequest(new InternalServerErrorResponse(response.Message, response.Result));
            }
        }
    }
}
