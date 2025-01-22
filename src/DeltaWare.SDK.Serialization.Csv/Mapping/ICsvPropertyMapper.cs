using System;
using System.Collections.Generic;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    internal interface ICsvPropertyMapper
    {
        IReadOnlyDictionary<int, PropertyInfo> CreatePropertyMappings(Type type, bool requiresSetter, IReadOnlyList<string?>? csvHeaders = null);
    }
}
