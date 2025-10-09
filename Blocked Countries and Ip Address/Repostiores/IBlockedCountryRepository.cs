using Blocked_Countries_and_Ip_Address.Entites;
using Blocked_Countries_and_Ip_Address.Repostiores.RegisterServices;
using Blocked_Countries_and_Ip_Address.Requests;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace Blocked_Countries_and_Ip_Address.Repostiores
{
    public interface IBlockedCountryRepository : ISingletonService
    {
        Task<BlockedCountry> AddBlockedCountryAsync(BlockCountryRequest country);
        Task<BlockedCountry> AddTemporalBlock(TemporalBlockRequest blockedCountry, int durationInMinutes);
        Task<bool> DeleteBlockedAsync(string countryCode);
        public Task<object> GetAllAsync(PaginationRequest pagination, string? filter);

    }

    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> storage = new(StringComparer.OrdinalIgnoreCase);
        public Task<BlockedCountry> AddBlockedCountryAsync(BlockCountryRequest country)
        {
            BlockedCountry blockedCountry = new BlockedCountry { CountryCode = country.CountryCode.ToUpper(), CountryName = country.CountryName };

            bool added = storage.TryAdd(blockedCountry.CountryCode, blockedCountry);
            if (added)
            {
                return Task.FromResult(blockedCountry);
            }
            else
                throw new Exception("Failed to add this country or already exist");
        }

        public Task<object> GetAllAsync(PaginationRequest pagination, string? filter)
        {
            List<BlockedCountry> blockedCountries = storage.Values.ToList();
            var total = blockedCountries.Count;

            if (filter != null) 
                blockedCountries = blockedCountries.Where(x => x.CountryName.Contains(filter) || x.CountryCode.Contains(filter.ToUpper())).ToList();

            var items = blockedCountries.Skip((pagination.PageIndex - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();

            return Task.FromResult<object>(new {total , pagination.PageIndex , pagination.PageSize , items});
        }

        public Task<bool> DeleteBlockedAsync(string countryCode)
        {
            return Task.FromResult(storage.Remove(countryCode,out _));

        }
        public async Task<BlockedCountry> AddTemporalBlock(TemporalBlockRequest blockedCountry, int durationInMinutes)
        {
            BlockedCountry country = new BlockedCountry()
            {
                CountryCode = blockedCountry.CountryCode.ToUpper(),
                CountryName = blockedCountry.CountryName,
                BlockedUntil = DateTime.UtcNow.AddMinutes(durationInMinutes)
            };

            return storage.AddOrUpdate(country.CountryCode, country, (key, oldvalue) => country);
        }
    }
}
