using Blocked_Countries_and_Ip_Address.Entites;
using Blocked_Countries_and_Ip_Address.Repostiores.RegisterServices;
using Blocked_Countries_and_Ip_Address.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Concurrent;

namespace Blocked_Countries_and_Ip_Address.Repostiores
{
    public interface IBlockedCountryRepository : ISingletonService
    {
        Task<BlockedCountry> AddAsync(BlockCountryRequest country);
        Task<bool> RemoveAsync(string countryCode);
        Task<List<BlockedCountry>> GetAllAsync();

    }

    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> storage = new(StringComparer.OrdinalIgnoreCase);
        public Task<BlockedCountry> AddAsync(BlockCountryRequest country)
        {
            BlockedCountry blockedCountry = new BlockedCountry { CountryCode = country.CountryCode };

            bool added = storage.TryAdd(country.CountryCode.ToUpper(), blockedCountry);
            if (added)
            {
                return Task.FromResult(blockedCountry);
            }
            else
                throw new Exception("Failed to add this country or already exist");
        }

        public Task<List<BlockedCountry>> GetAllAsync()
        {
            return Task.FromResult(storage.Values.ToList());
        }

        public Task<bool> RemoveAsync(string countryCode)
        {
            throw new NotImplementedException();
        }
    }
}
