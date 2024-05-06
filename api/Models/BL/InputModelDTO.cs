using Newtonsoft.Json;
using System.Globalization;

namespace api.Models.BL
{
    public class InputModelDTO
    {
        public int? ID { get; set; }
        public PersonDetailsDTO? Applicant { get; set; }
        public FamilyMemberDTO[]? FamilyMembers { get; set; }
    }
}
