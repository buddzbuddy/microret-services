namespace api.Models.BL
{
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
}
