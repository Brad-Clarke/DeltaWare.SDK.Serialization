using System;
using DeltaWare.SDK.Serialization.Csv.Serialization.Transformers;
using System.Diagnostics;
using System.Reflection;
using DeltaWare.SDK.Serialization.Csv.Validation;

namespace DeltaWare.SDK.Serialization.Csv.Mapping
{
    [DebuggerDisplay("[{Index}]{Property.Name}")]
    public readonly struct PropertyMapping(PropertyInfo property, string headerName, int index, ITransformer transformer, IFormatProvider formatProvider)
    {
        public PropertyInfo Property { get; } = property;

        public string HeaderName { get; } = headerName;

        public int Index { get; } = index;

        public ITransformer Transformer { get; } = transformer;

        public IFormatProvider FormatProvider { get; } = formatProvider;

        public IPropertyValidator PropertyValidator { get; } = new DefaultPropertyValidator(property);
    }
}
