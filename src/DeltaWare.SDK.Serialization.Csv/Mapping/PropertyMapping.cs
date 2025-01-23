using System.Diagnostics;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    [DebuggerDisplay("[{Index}]{Property.Name}")]
    public sealed class PropertyMapping(PropertyInfo property, string headerName, int index)
    {
        public PropertyInfo Property { get; } = property;

        public string HeaderName { get; } = headerName;

        public int Index { get; } = index;
    }
}
