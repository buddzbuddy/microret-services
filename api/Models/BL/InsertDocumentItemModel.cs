namespace api.Models.BL
{
    public class InsertDocumentItemModel
    {
        public required Guid Id { get; set; }
        public required Guid Def_Id { get; set; }
        public required DateTime Created { get; set; }
        public required Guid UserId { get; set; }
        public required Guid Organization_Id { get; set; }
        public required Guid Org_Position_Id { get; set; }
        public DateTime Last_Modified => Created;
    }
}
