namespace Blocked_Countries_and_Ip_Address.Entites
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public DateTime? BlockedUntil { get; set; }

    }
}
