using System;
using System.Collections.Generic;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    internal interface ICsvPropertyMapper
    {
        IEnumerable<PropertyMapping> CreatePropertyMappings(Type type, bool requiresSetter, IReadOnlyList<string?>? csvHeaders = null);
    }
}
