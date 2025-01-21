using System.Diagnostics;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    [DebuggerDisplay("[{Index}]{Property.Name}")]
    public sealed class PropertyMapping(PropertyInfo property, int index)
    {
        public PropertyInfo Property { get; } = property;

        public int Index { get; } = index;
    }
}
