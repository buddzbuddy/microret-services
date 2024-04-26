namespace api.Models.BL
{
    public class DocStateItemModel
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public required Guid Document_Id { get; set; }
        public required DateTime Created { get; set; }
        public required Guid Worker_Id { get; set; }
        public DateTime Expired => DateTime.MaxValue.Date;
        public required Guid State_Type_Id { get; set; }
    }
}
