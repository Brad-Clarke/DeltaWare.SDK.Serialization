using System.Diagnostics;
using System.Reflection;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    [DebuggerDisplay("[{Index}]{Property.Name} - Ignore:{Ignore}")]
    public sealed class PropertyMapping(PropertyInfo property, int index, bool ignore)
    {
        public PropertyInfo Property { get; } = property;

        public int Index { get; } = index;

        public bool Ignore { get; } = ignore;
    }
}
