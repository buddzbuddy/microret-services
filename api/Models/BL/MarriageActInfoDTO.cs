namespace api.Models.BL
{
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
}
