using System;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Header
{
    public interface IHeaderHandler
    {
        string[] GetHeaderNames(PropertyInfo[] indexedProperties);

        PropertyInfo[] BuildHeaderIndex(Type type, int columnCount, string[] headers = null);

        PropertyInfo[] GetIndexedProperties(Type type);
    }
}
