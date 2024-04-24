using api.Models.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace api.Extensions
{
    public static class CissaExtensions
    {
        public static Guid GetDefId<T>(string propName)
        {
            var propInfo = typeof(T).GetProperty(propName);
            if(propInfo == null) throw new ArgumentNullException(nameof(propInfo),
                $"Property not found: {propName}");
            var attributes = propInfo.GetCustomAttributes(false).OfType<DefIdAttribute>();

            if (attributes.Any())
            {
                return attributes.First().Value;
            }
            else
                throw new ArgumentException("Property is not assigned to DefId attribute");
        }
        public static Guid GetDefId<T>()
        {
            var attributes = typeof(T).GetCustomAttributes(false).OfType<DefIdAttribute>();

            if (attributes.Any())
            {
                return attributes.First().Value;
            }
            else
                throw new ArgumentException("Class is not assigned to DefId attribute");
        }
    }
}
