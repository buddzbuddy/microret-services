namespace api.Models.BL
{
    public class PassportDataInfoDTO : PassportOnlyDTO
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public string? SurnameLatin { get; set; }
        public string? NameLatin { get; set; }
        public string? PatronymicLatin { get;set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        
        public string? VoidMotiv { get; set; }
        public string? FamilyStatus { get; set; }
        public string? Gender { get; set; }
        public string? AddressRegion { get; set; }
        public string? AddressLocality { get; set; }
        public string? AddressStreet { get; set; }
        public string? AddressHouse { get; set; }
        public string? AddressBuilding { get; set; }
        public string? AddressApartment { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? AreaId { get; set; }
        public int? SubareaId { get; set; }
        public int? StreetId { get; set; }
        public int? HouseId { get; set; }
        public string? ApplicantType { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
