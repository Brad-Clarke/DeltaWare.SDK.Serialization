using System;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Extensions
{
    internal static class TypeExtensions
    {
        public static PropertyInfo[] GetPublicProperties(this Type type)
            => type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public static bool ImplementsInterface<T>(this Type type) where T : class
            => type.GetInterfaces().Contains(typeof(T));

        public static bool IsNullableType(this Type type)
            => Nullable.GetUnderlyingType(type) != null || !type.IsValueType;

    }
}
