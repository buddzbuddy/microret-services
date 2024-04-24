using api.Models.Enums;

namespace api.Models.BL
{
    public class AttributeBaseModel<T>
    {
        public required Guid Document_Id { get; set; }
        public required Guid Def_Id { get; set; }
        public required DateTime Created { get; set; }
        public DateTime Expired => DateTime.MaxValue.Date;
        public required Guid UserId { get; set; }
        public required T Value { get; set; }
        public int TypeCode
        {
            get
            {
                if (Value is int) return (int)AttributeTypeCode.INT;
                else if (Value is Guid) return (int)AttributeTypeCode.GUID;
                else if (Value is string) return (int)AttributeTypeCode.STRING;
                else if (Value is decimal) return (int)AttributeTypeCode.DECIMAL;
                else if (Value is DateTime) return (int)AttributeTypeCode.DATETIME;
                throw new InvalidCastException($"Тип значения атрибута не распознан! typename: {typeof(T).Name}");
            }
        }
    }
}
