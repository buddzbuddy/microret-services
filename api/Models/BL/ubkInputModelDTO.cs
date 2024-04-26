using Newtonsoft.Json;
using System.Globalization;

namespace api.Models.BL
{
    public class ubkInputModelDTO
    {
        public int? ID { get; set; }
        public PersonDetailsDTO? Applicant { get; set; }
        public FamilyMemberDTO[]? FamilyMembers { get; set; }

        public class FamilyMemberDTO : PersonDetailsDTO
        {
            public new string? pin { get; set; }
            public string? lastname { get; set; }
            public string? firstname { get; set; }
            public string? patronymic { get; set; }
            public string? role { get; set; }
            public int? roleId { get; set; }

            public BirthActByPinInfoDTO? BirthActByPinInfo { get; set; }
        }

    }
}
