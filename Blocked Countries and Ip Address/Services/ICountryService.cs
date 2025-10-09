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
        Task<object> GetAllAsync(PaginationRequest pagination, string? filter);
        Task<BlockedCountry> TemoralBlockCountry(TemporalBlockRequest blockRequest, int durationInMinutes);
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
        public async Task<object> GetAllAsync(PaginationRequest pagination, string? filter) => await _blockedCountryRepository.GetAllAsync(pagination,filter);

        public async Task<bool> DeleteAsync(string countryCode) => await _blockedCountryRepository.DeleteBlockedAsync(countryCode);

        public async Task<BlockedCountry> TemoralBlockCountry(TemporalBlockRequest blockRequest, int durationInMinutes)
        {
            if (durationInMinutes > 1 && durationInMinutes < 1440)
                return await _blockedCountryRepository.AddTemporalBlock(blockRequest, durationInMinutes);
            else
                throw new InvalidOperationException("Duration must be between 1 and 1440 minutes");
        }
    }
}
