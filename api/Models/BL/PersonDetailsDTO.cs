namespace api.Models.BL
{
    public class PersonDetailsDTO
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
