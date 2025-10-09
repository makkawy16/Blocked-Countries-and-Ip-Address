namespace Blocked_Countries_and_Ip_Address.Requests
{
    public class BlockCountryRequest
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
    public record TemporalBlockRequest(string CountryCode, string CountryName, int DurationInMinutes);

}
