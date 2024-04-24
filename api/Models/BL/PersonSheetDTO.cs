using api.Models.Annotations;

namespace api.Models.BL
{
    [DefId("{6F5B8A06-361E-4559-8A53-9CB480A9B16C}")]
    public class PersonSheetDTO
    {
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        [DefId("{58FD799D-8CE0-4AAD-9583-126D544F0776}")]
        public required string PIN { get; set; }
        [DefId("{FC4BCDB0-8EAF-47FC-B176-29C11046FE89}")]
        public required string LastName { get; set; }
        [DefId("{FAFE85DF-E3A3-47C7-B11C-5D9C5727BEBD}")]
        public required string FirstName { get; set; }
        [DefId("{F7303081-08B0-428A-ABAE-01AE22F5209E}")]
        public required string? MiddleName { get; set; }
        [DefId("{4ED594FB-5E2F-4451-B24F-07E9DDE585E2}")]
        public required DateTime BirthDate { get; set; }
        [DefId("{D05054EC-2AAE-4DF5-BAA8-4E59E837E4BA}")]
        public required Guid Sex { get; set; }
        [DefId("{86305A88-825A-4EAA-B9E2-7B2FE167CD9B}")]
        public required Guid DocumentType { get; set; }
        [DefId("{C958BA4B-332E-4394-A5BC-F62AFD56BA92}")]
        public required Guid PassportSeries { get; set; }
        [DefId("{B23C0122-921B-4241-AFF3-2CA966A39C9E}")]
        public required string PassportNo { get; set; }
        [DefId("{7B28ED97-657A-42C8-932A-A0FEDF403185}")]
        public required DateTime PassportDate { get; set; }
        [DefId("{17D50A0F-9232-4915-AC33-BE57B7A670CA}")]
        public required string? PassportOrg { get; set; }
        [DefId("{3DCC284C-9254-4249-B110-0F277AE7476B}")]
        public required DateTime? PassportExpiryDate { get; set; }
    }
}
