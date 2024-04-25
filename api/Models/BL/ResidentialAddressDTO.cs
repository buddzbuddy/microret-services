namespace api.Models.BL
{
    public class ResidentialAddressDTO
    {
        public string? State { get; set; }
        public int? StateId { get; set; }
        public string? StateCode { get; set; }
        public string? Region { get; set; }
        public int? RegionId { get; set; }
        public string? RegionCode { get; set; }
        public string? District { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictCode { get; set; }
        public string? City { get; set; }
        public int? CityId { get; set; }
        public string? CityCode { get; set; }
        public string? Street { get; set; }
        public int? StreetId { get; set; }
        public string? StreetCode { get; set; }
        public string? House { get; set; }
        public string? Flat { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
