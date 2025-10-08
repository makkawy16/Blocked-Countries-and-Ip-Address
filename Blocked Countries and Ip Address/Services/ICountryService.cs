using Blocked_Countries_and_Ip_Address.Entites;
using Blocked_Countries_and_Ip_Address.Repostiores;
using Blocked_Countries_and_Ip_Address.Repostiores.RegisterServices;
using Blocked_Countries_and_Ip_Address.Requests;

namespace Blocked_Countries_and_Ip_Address.Services
{
    public interface ICountryService : IScopedService
    {
        Task<BlockedCountry> AddBlockCountry(BlockCountryRequest country);
        Task<bool> DeleteAsync(string countryCode);
        Task<object> GetAllAsync(PaginationRequest pagination);
    }
    public class CountryService : ICountryService
    {
        private readonly IBlockedCountryRepository _blockedCountryRepository;

        public CountryService(IBlockedCountryRepository blockedCountryRepository)
        {
            _blockedCountryRepository = blockedCountryRepository;
        }

        public async Task<BlockedCountry> AddBlockCountry (BlockCountryRequest country)
        {
            return await _blockedCountryRepository.AddBlockedCountryAsync(country);
        }
        public async Task<object> GetAllAsync(PaginationRequest pagination) => await _blockedCountryRepository.GetAllAsync(pagination);

        public async Task<bool> DeleteAsync(string countryCode) => await _blockedCountryRepository.DeleteBlockedAsync(countryCode);
    }
}
