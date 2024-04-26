namespace api.Models.BL
{
    public class setApplicationResultDTO
    {
        public Guid? appId { get; set; }
        public string? Decision { get; set; }
        public string? RejectionReason { get; set; }
    }
}
