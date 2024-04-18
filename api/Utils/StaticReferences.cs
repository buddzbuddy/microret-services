using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace api.Utils
{
    public static class StaticReferences
    {
        public const int ADULT_AGE_STARTS_FROM = 18;
        public const int PIN_LENGTH = 14;
        public const string PIN_BIRTHDATE_SECTION_FORMAT = "ddMMyyyy";
        public const int MINIMUM_NAME_LENGTH = 2;
        public const string MALE_ENDS_WITH = "УУЛУ";
        public const string FEMALE_ENDS_WITH = "КЫЗЫ";
        public const int PASSPORT_DEFAULT_VALID_YEARS = 10;
        public const int MEN_RETIREMENT_AGE = 63;
        public const int WOMEN_RETIREMENT_AGE = 58;
        public const string UNEMPLOYEE_STATUS_NAME = "Официальный безработный";

        public static bool IsAnyNullOrEmpty<T>(T? myObject, out string[] foundNullPropNames,
            string[]? onlySpecificProps, bool exclude = false)
        {
            if(myObject == null) throw new ArgumentException(
                $"Provided object to check his properties null or empty is null himself." +
                $" ObjectTypeName: {typeof(T).Name}");
            var list = new List<string>();
            var propsForCheck = myObject.GetType().GetProperties().AsEnumerable();
            if(!propsForCheck.Any()) throw new ArgumentException(
                $"Provided object has no any properties" +
                $" to check null or empty. ObjectTypeName: {myObject.GetType().Name}");
            if (onlySpecificProps != null && onlySpecificProps.Length > 0)
            {
                if (exclude)
                    propsForCheck = propsForCheck.Where(x => !onlySpecificProps.Select(x => x.ToUpper())
                    .Contains(x.Name.ToUpper()));
                else
                    propsForCheck = propsForCheck.Where(x => onlySpecificProps.Select(x => x.ToUpper())
                .Contains(x.Name.ToUpper()));
            }
            if (!propsForCheck.Any()) throw new ArgumentException(
                $"Provided object has no filtered properties[{string.Join(',',onlySpecificProps ?? Array.Empty<string>())}]" +
                $" to check null or empty. ObjectTypeName: {myObject.GetType().Name}");
            foreach (PropertyInfo pi in propsForCheck)
            {
                if (pi.PropertyType == typeof(string))
                {
                    if (string.IsNullOrEmpty(pi.GetValue(myObject) as string))
                    {
                        list.Add(pi.Name);
                    }
                }
                else if (pi.GetValue(myObject) == null) list.Add(pi.Name);
            }
            foundNullPropNames = list.ToArray();
            return foundNullPropNames.Length > 0;
        }
        public static void CheckNulls<T>(T? myObject, params string[]? onlySpecificProps)
        {
            if (IsAnyNullOrEmpty(myObject, out string[] foundNullPropNames, onlySpecificProps))
            {
                if (foundNullPropNames.Length > 0)
                {
                    throw new ArgumentNullException(string.Join(',', foundNullPropNames),
                        ErrorMessageResource.NullDataProvidedError);
                }
                else throw new InvalidOperationException(
                    $"Error made by conflict of result for" +
                    $" {nameof(IsAnyNullOrEmpty)}." +
                    $" Please contact the administrator");
            }
        }
        
        public static void CheckNullsWithExcludeProps<T>(T? myObject,
            params string[]? excludeSpecificProps)
        {
            if (IsAnyNullOrEmpty(myObject, out string[] foundNullPropNames, excludeSpecificProps, true))
            {
                if (foundNullPropNames.Length > 0)
                {
                    throw new ArgumentNullException(string.Join(',', foundNullPropNames),
                        ErrorMessageResource.NullDataProvidedError);
                }
                else throw new InvalidOperationException(
                    $"Error made by conflict of result for" +
                    $" {nameof(IsAnyNullOrEmpty)}." +
                    $" Please contact the administrator");
            }
        }
    }
}
