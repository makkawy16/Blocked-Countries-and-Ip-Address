using Blocked_Countries_and_Ip_Address.Entites;
using Blocked_Countries_and_Ip_Address.Repostiores.RegisterServices;
using Blocked_Countries_and_Ip_Address.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace Blocked_Countries_and_Ip_Address.Repostiores
{
    public interface IBlockedCountryRepository : ISingletonService
    {
        Task<BlockedCountry> AddBlockedCountryAsync(BlockCountryRequest country);
        Task<bool> DeleteBlockedAsync(string countryCode);
        public Task<object> GetAllAsync(PaginationRequest pagination);

    }

    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> storage = new(StringComparer.OrdinalIgnoreCase);
        public Task<BlockedCountry> AddBlockedCountryAsync(BlockCountryRequest country)
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

        public Task<object> GetAllAsync(PaginationRequest pagination)
        {
            List<BlockedCountry> blockedCountries = storage.Values.ToList();
            var total = blockedCountries.Count;
            var items = blockedCountries.Skip((pagination.PageIndex - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return Task.FromResult<object>(new {total , pagination.PageIndex , pagination.PageSize , items});
        }

        public Task<bool> DeleteBlockedAsync(string countryCode)
        {
            return Task.FromResult(storage.Remove(countryCode,out _));

        }
    }
}
