namespace api.Models.BL
{
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
}
