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

        [HttpPost]
        public async Task<IActionResult> AddBlockedCountry([FromBody] BlockCountryRequest request) {
            BlockedCountry result = await _countryService.AddBlockCountry(request);
            return Created(string.Empty, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlockedCountries() {
            List<BlockedCountry> blockedCountries = await _countryService.GetAllAsync();
            return Ok(blockedCountries);
        }
    }
}
