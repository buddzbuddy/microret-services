using api.Models.Annotations;

namespace api.Models.BL
{
    [DefId("{04D25808-6DE9-42F5-8855-6F68A94A224C}")]
    public class SocialApplicationDTO
    {
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        [DefId("{F4D191E9-64F5-4D3D-9F6D-04C8171CB692}")]
        public required string RegNo { get; set; }
        [DefId("{E36E02FA-BDD3-4E3B-978D-B5BB50B7BCB7}")]
        public required DateTime RegDate { get; set; }
        [DefId("{C00B3DD8-136A-4A1A-946F-F3B47EA5B847}")]
        public required Guid ApplyType { get; set; }
        [DefId("{BD22FDAB-7C65-46B4-922B-D9DE61C152E5}")]
        public required PersonSheetDTO Applicant { get; set; }
        [DefId("{DF98548C-38E9-49AA-934B-5ECAD06955F0}")]
        public required Guid? PaymentType { get; set; }
        [DefId("{1BCF8AA6-7662-4128-BA27-FF4EE0125C76}")]
        public string? Town { get; set; }
        [DefId("{F9FFD608-CE1A-436F-B8BA-26B7E7D9586B}")]
        public string? Street { get; set; }
        [DefId("{F77B7847-956B-4336-A63A-147714900A2B}")]
        public string? House { get; set; }
        [DefId("{BED72967-8C42-4401-AD8C-C14473D18909}")]
        public string? Apartment { get; set; }
        [DefId("{321C8912-DFDE-4175-879A-3EFE3A08004E}")]
        public string? MobilePhone { get; set; }
        [DefId("{AB44BA4D-10FF-4F7A-B9E9-B58204F9D379}")]
        public int AmountOfAdults => 1;
        [DefId("{1790BEF8-DA15-4CF9-A817-93BDA93C354B}")]
        public int FamilyMemberCount => 1;
    }
}
