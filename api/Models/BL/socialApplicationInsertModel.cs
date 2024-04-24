namespace api.Models.BL
{
    public class socialApplicationInsertModel
    {
        public required Guid OrgId { get; set; }
        public required Guid PositionId { get; set; }
        public required Guid UserId { get; set; }

        public List<InsertDocumentItemModel> insertDocuments { get; set; } = new();

        public List<AttributeBaseModel<Guid>> docAttributes { get; set; } = new();
        public List<AttributeBaseModel<int>> intAttributes { get; set; } = new();
        public List<AttributeBaseModel<Guid>> enumAttributes { get; set; } = new();
        public List<AttributeBaseModel<DateTime>> dateAttributes { get; set; } = new();
        public List<AttributeBaseModel<string>> textAttributes { get; set; } = new();
        public DocStateItemModel? docState { get; set; }
    }
}
