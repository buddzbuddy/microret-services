namespace api.Models.BL
{
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
}
