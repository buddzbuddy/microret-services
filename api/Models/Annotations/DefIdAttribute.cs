namespace api.Models.Annotations
{
    public class DefIdAttribute : Attribute
    {
        public readonly Guid Value;
        public DefIdAttribute(string defId)
        {
            Value = Guid.Parse(defId);
        }
    }
}
