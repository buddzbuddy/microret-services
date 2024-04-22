using Newtonsoft.Json;
using System.Globalization;

namespace api.Models.BL
{
    public class ubkInputJsonDTO
    {
        public ApplicantDTO? Applicant { get; set; }
        public FamilyMemberDTO[]? FamilyMembers { get; set; }

        public class ApplicantDTO : PersonDetailsInfo
        {
            
        }

        public class FamilyMemberDTO : PersonDetailsInfo
        {
            public new string? pin { get; set; }
            public string? lastname { get; set; }
            public string? firstname { get; set; }
            public string? patronymic { get; set; }
            public string? role { get; set; }
            public int? roleId { get; set; }

            public BirthActByPinInfoDTO? BirthActByPinInfo { get; set; }
        }

        public abstract class PersonDetailsInfo
        {
            public string? pin { get; set; }
            public PassportDataInfoDTO? PassportDataInfo { get; set; }
            public MarriageActInfoDTO? MarriageActInfo { get; set; }
            public ResidentialAddressDTO? ResidentialAddress { get; set; }
            public WorkPeriodInfoDTO? WorkPeriodInfo { get; set; }
            public UnemployedStatusInfoDTO? UnemployedStatusInfo { get; set; }
            public PensionInfoDTO? PensionInfo { get; set; }
            public MSECDetailsInfoDTO? MSECDetailsInfo { get; set; }
            public RealEstateInfoDTO[]? RealEstateInfoList { get; set; }
            public AnimalDataDTO[]? AnimalDataList { get; set; }
            public CarDTO[]? Cars { get; set; }
        }
    }
    

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
        public int? PassportServiceDataID { get; set; }
        public string? ApplicantType { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public abstract class PassportOnlyDTO
    {
        public string? Pin { get; set; }
        public string? PassportSeries { get; set; }
        public string? PassportNumber { get; set; }
        public string? VoidStatus { get; set; }
        public string? PassportAuthority { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
    public class MarriageActInfoDTO
    {
        public string? Act { get; set; }
        public CrtfDTO? Crtf { get; set; }
        public GroomBrideDTO? Groom {  get; set; }
        public GroomBrideDTO? Bride { get; set; }
        public class CrtfDTO
        {
            public DateTime? DocDate { get;set; }
            public string? DocNumber { get; set; }
            public string? DocSeries { get; set; }
            public string? GovUnit { get; set; }
            public bool? IsDuplicate { get; set; }
            public string? Surname { get; set; }
            public string? FirstName { get; set; }
            public string? Patronymic { get; set; }
        }
        public class GroomBrideDTO
        {
            public string? Pin { get; set; }
            public string? Surname { get; set; }
            public string? FirstName { get; set; }
            public string? Patronymic { get; set; }
            public string? NewSurname { get; set; }
            public string? NewFirstName { get; set; }
            public string? NewPatronymic { get; set; }
            public string? Nationality { get; set; }
            public string? Citizenship { get; set; }
            public string? PlaceOfBirth { get; set; }
        }
    }
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
    public class UnemployedStatusInfoDTO
    {
        public string? Status { get; set; }
        public bool? EligibleForBenefits { get; set; }
    }
    public class PensionInfoDTO
    {
        public DateTime? Date { get; set; }
        public string? State { get; set; }
        public string? Pin { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? FullName { get; set; }
        public string? Issuer { get; set; }
    }
    public class BirthActByPinInfoDTO
    {
        public DateTime? ActDate { get; set; }
        public string? ActNumber { get; set; }
        public string? ActGovUnit { get; set; }
        public string? ChildSurname { get; set; }
        public string? ChildFirstName { get; set; }
        public string? ChildPatronymic { get; set; }
        public int? ChildGender { get; set; }
        public string? ChildPlaceOfBirth { get; set; }
        public string? MotherPin { get; set; }
        public string? MotherSurname { get; set; }
        public string? MotherFirstName { get; set;}
        public string? MotherPatronymic { get; set;}
        public string? MotherNationality { get; set; }
        public string? MotherCitizenship { get; set; }
        public string? FatherPin { get; set; }
        public string? FatherSurname { get;set; }
        public string? FatherFirstName { get; set;}
        public string? FatherPatronymic { get; set; }
        public string? FatherNationality { get;set; }
        public string? FatherCitizenship { get; set; }
    }
    public class MSECDetailsInfoDTO
    {
        public string? OrganizationName { get; set; }
        public DateTime? ExaminationDate { get; set; }
        public string? ExaminationType { get; set; }
        public string? DisabilityGroup { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? TimeOfDisability { get; set; }
        public string? ReExamination { get; set; }
        public string? StatusCode { get; set; }
        public bool? InAbsentia { get; set; }
        public bool? IsDeathPeriod { get; set; }
    }
    public class RealEstateInfoDTO
    {
        public string? PropCode { get; set; }
        public string? Address { get; set; }
        public string? Owner { get; set; }
        public string? Pin { get; set; }
        public string? DocNum { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? TermDate { get; set; }
    }
    public class AnimalDataDTO
    {
        public string? Type { get; set; }
        public string? Gender { get; set; }
        public string? Age { get; set; }
    }
    public class CarDTO
    {
        public string? GovPlate { get; set; }
        public string? CarTypeName { get; set; }
        public string? BodyType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Steering { get; set; }
        public int? Year { get; set; }
        public string? Color { get; set; }
        public string? BodyNo { get; set; }
        public string? Vin { get; set; }
        public int? EngineVolume { get; set; }
        public string? DateFrom { get; set; }
    }
    public class WorkPeriodInfoDTO
    {
        public string? State { get; set; }
        public string? PIN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Issuer { get; set; }
        public Item[]? WorkPeriods { get; set; }
        public class Item
        {
            public string? PIN_LSS { get; set; }
            public string? Payer { get; set; }
            public string? INN { get; set;}
            public long? NumSF { get; set;}
            /// <summary>
            /// Use FORMAT
            /// </summary>
            public string? DateBegin { get;set; }
            public string? DateEnd { get; set; }
            public double? Sum { get; set; }
        }
    }
}
