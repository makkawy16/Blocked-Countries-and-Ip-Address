using Blocked_Countries_and_Ip_Address.Entites;
using Blocked_Countries_and_Ip_Address.Requests;
using Blocked_Countries_and_Ip_Address.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocked_Countries_and_Ip_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> AddBlockedCountry([FromBody] BlockCountryRequest request) {
            BlockedCountry result = await _countryService.AddBlockCountry(request);
            return Created(string.Empty, result);
        }

        [HttpGet("blocked")]
        public async Task<IActionResult> GetAllBlockedCountries([FromQuery] PaginationRequest pagination,[FromQuery] string? filter = null) {
            object blockedCountries = await _countryService.GetAllAsync(pagination,filter);
            return Ok(blockedCountries);
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> DeleteBlockedCountry(string countryCode)
        {
            bool IsDeleted = await _countryService.DeleteAsync(countryCode);
            if (IsDeleted) 
                return Ok(IsDeleted);
            else
                return NotFound();
        }

        [HttpPost("temporal-block")]
        public async Task<IActionResult> TempralBlock([FromBody] TemporalBlockRequest temporalBlockRequest)
        {
            var reuslt = _countryService.TemoralBlockCountry(temporalBlockRequest,temporalBlockRequest.DurationMinutes);
            return Ok(reuslt);
        }
    }
}
