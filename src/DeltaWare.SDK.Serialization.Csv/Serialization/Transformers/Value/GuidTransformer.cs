using System;

namespace DeltaWare.SDK.Serialization.Csv.Serialization.Transformers.Value
{
    internal sealed class GuidTransformer : TransformerBase<Guid>
    {
        protected override Guid TransformToObjectType(string value, IFormatProvider formatProvider)
            => Guid.Parse(value, formatProvider);

        protected override string TransformFromObjectType(Guid value, IFormatProvider formatProvider)
            => value.ToString(null, formatProvider);
    }
}
