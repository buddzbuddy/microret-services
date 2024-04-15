﻿using Microsoft.SqlServer.Server;
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
        
        public static bool IsAnyNullOrEmpty<T>(T? myObject, out string[] foundNullPropNames, params string[]? onlySpecificProps)
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
                propsForCheck = propsForCheck.Where(x => onlySpecificProps.Select(
                    x => x.ToUpper())
                .Contains(x.Name.ToUpper()));
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
                    $" {nameof(StaticReferences.IsAnyNullOrEmpty)}." +
                    $" Please contact the administrator");
            }
        }

    }
}
