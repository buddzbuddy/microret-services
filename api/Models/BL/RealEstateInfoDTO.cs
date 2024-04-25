namespace api.Models.BL
{
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
}
